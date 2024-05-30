using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Bootcamp.Repository.Repositories;
using Bootcamp.Repository.Token;
using Bootcamp.Service.SharedDto;
using Bootcamp.Service.Token;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace Bootcamp.Service.Users
{
    public class UserService(
        IGenericRepository<RefreshToken> _refreshTokenRepository,
        UserManager<AppUser> _userManager,
        RoleManager<AppRole> _roleManager,
        IOptions<CustomTokenOptions> _customTokenOptions,
        IOptions<Clients> _clients,
        IUnitOfWork _unitOfWork)
    {
        // signup
        public async Task<ResponseModelDto<Guid>> SignUp(SignUpRequestDto request)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                Name = request.Name,
                Surname = request.Lastname,
                BirthDate = request.BirthDate
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return ResponseModelDto<Guid>.Fail(result.Errors.Select(x => x.Description).ToList());
            }

            //if (request.BirthDate.HasValue)
            //{
            //    await userManager.AddClaimAsync(user,
            //        new Claim(ClaimTypes.DateOfBirth, user.BirthDate.Value.ToShortDateString()));
            //}


            return ResponseModelDto<Guid>.Success(user.Id, HttpStatusCode.Created);
        }


        // signin
        public async Task<ResponseModelDto<TokenResponseDto>> SignIn(SignInRequestDto request)

        {
            // Fast fail
            // Guard clauses
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return ResponseModelDto<TokenResponseDto>.Fail("Email or Password is wrong", HttpStatusCode.NotFound);
            }


            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return ResponseModelDto<TokenResponseDto>.Fail("Email or Password is wrong", HttpStatusCode.BadRequest);
            }

            var userClaims = CreateUserClaims(user, _customTokenOptions.Value);
            var accessToken = CreateAccessToken(_customTokenOptions.Value, userClaims.Result);
            var refreshToken = await CreateOrUpdateRefreshToken(user.Id);

            return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(accessToken, refreshToken));
        }

        private async Task<string> CreateOrUpdateRefreshToken(Guid userId)
        {
            var hasRefreshToken = await _refreshTokenRepository.Where(rt => rt.UserId == userId).SingleOrDefaultAsync();
            if (hasRefreshToken != null)
            {
                hasRefreshToken = new RefreshToken()
                {
                    Code = new Guid(),
                    UserId = userId,
                    Expire = DateTime.Now.AddDays(_customTokenOptions.Value.RefreshTokenExpireByDay)
                };

                await _refreshTokenRepository.Create(hasRefreshToken);
            }
            else
            {
                hasRefreshToken!.Code = Guid.NewGuid();
                hasRefreshToken!.Expire = DateTime.Now.AddDays(_customTokenOptions.Value.RefreshTokenExpireByDay);

                await _refreshTokenRepository.Update(hasRefreshToken);
            }

            await _unitOfWork.CommitAsync();

            return hasRefreshToken.Code.ToString();
        }

        private string CreateAccessToken(CustomTokenOptions customTokenOptions, List<Claim> claimList)
        {
            var tokenExpire = DateTime.Now.AddHours(customTokenOptions.ExpireByHour);


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(customTokenOptions.Signature));

            //DateTimeOffset.Now.ToUnixTimeSeconds()
            var jwtToken = new JwtSecurityToken(
                claims: claimList,
                expires: tokenExpire,
                issuer: customTokenOptions.Issuer,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));


            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);

            return token;
        }

        private async Task<List<Claim>> CreateUserClaims(AppUser appUser, CustomTokenOptions customTokenOptions)
        {
            var userClaimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(ClaimTypes.Name, appUser.UserName!)
            };


            customTokenOptions.Audience.ToList()
                .ForEach(x => { userClaimList.Add(new Claim(JwtRegisteredClaimNames.Aud, x)); });

            var userRoles = await _userManager.GetRolesAsync(appUser);

            foreach (var userRole in userRoles)
            {
                userClaimList.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var userClaims = await _userManager.GetClaimsAsync(appUser);


            foreach (var userClaim in userClaims)
            {
                userClaimList.Add(new Claim(userClaim.Type, userClaim.Value));
            }


            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role is null)
                {
                    continue;
                }


                var roleClaim = await _roleManager.GetClaimsAsync(role);

                foreach (var roleAsClaim in roleClaim)
                {
                    userClaimList.Add(roleAsClaim);
                }
            }

            return userClaimList;
        }
    }
}
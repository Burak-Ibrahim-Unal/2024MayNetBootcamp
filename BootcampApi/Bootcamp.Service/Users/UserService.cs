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
        IOptions<CustomTokenOptions> _tokenOptions,
        IOptions<Clients> _clients)
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

            // userId
            // userName
            // roller
            // userClaim => 
            // role claim => permission
            var userClaimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };


            _tokenOptions.Value.Audience.ToList()
                .ForEach(x => { userClaimList.Add(new Claim(JwtRegisteredClaimNames.Aud, x)); });

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                userClaimList.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var userClaims = await _userManager.GetClaimsAsync(user);


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


            var tokenExpire = DateTime.Now.AddHours(_tokenOptions.Value.ExpireByHour);


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Value.Signature));


            //DateTimeOffset.Now.ToUnixTimeSeconds()
            var jwtToken = new JwtSecurityToken(
                claims: userClaimList,
                expires: tokenExpire,
                issuer: _tokenOptions.Value.Issuer,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));


            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);


            return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token));
        }
    }
}
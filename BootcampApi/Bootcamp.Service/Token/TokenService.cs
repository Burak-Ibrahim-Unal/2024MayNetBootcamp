using Bootcamp.Service.SharedDto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bootcamp.Service.Token
{
    public class TokenService(IOptions<CustomTokenOptions> customTokenOptions, IOptions<Clients> clients) : ITokenService
    {
        // options design pattern > read settings from appsettings with type safety (like typescript)
        public Task<ResponseModelDto<TokenResponseDto>> CreateClientAccessToken(GetAccessTokenRequestDto request)
        {
            if (!clients.Value.Items.Any(x => x.Id == request.ClientId && x.Secret == request.ClientSecret))
            {
                return Task.FromResult(
                    ResponseModelDto<TokenResponseDto>.Fail("Client not found"));
            }


            var claims = new List<Claim>()
            {
                new Claim("clientId", request.ClientId)
            };
            var tokenExpire = DateTime.Now.AddHours(customTokenOptions.Value.ExpireByHour);


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(customTokenOptions.Value.Signature));


            //DateTimeOffset.Now.ToUnixTimeSeconds()
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpire,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));


            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);


            return Task.FromResult(ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token)));
        }
    }
}
﻿using Bootcamp.Service.SharedDto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bootcamp.Service.Token
{
    public class TokenService(IOptions<CustomTokenOptions> tokenOptions, IOptions<Clients> clients) : ITokenService
    {
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

            tokenOptions.Value.Audience.ToList()
                .ForEach(x => 
                { 
                    claims.Add(new Claim(JwtRegisteredClaimNames.Aud, x)); 
                });

            var tokenExpire = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Signature));


            //DateTimeOffset.Now.ToUnixTimeSeconds()
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpire,
                issuer: tokenOptions.Value.Issuer,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature));


            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);


            return Task.FromResult(ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token)));
        }

        public Task<ResponseModelDto<TokenResponseDto>> CreateAccessTokenWithRefreshToken()
        {

        }
    }
}
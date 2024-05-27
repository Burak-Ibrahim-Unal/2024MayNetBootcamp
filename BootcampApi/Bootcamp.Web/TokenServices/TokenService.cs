using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bootcamp.Web.Models;
using Bootcamp.Web.Signin;
using Bootcamp.Web.Users.Signin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Bootcamp.Web.TokenServices
{
    public class TokenService(
        HttpClient _httpClient,
        IOptions<CustomTokenOptions> _tokenOptions,
        IMemoryCache _memoryCache)

    {
        private const string TokenKey = "client_credential:access_token";

        public async Task<(bool isSuccess, string? token, List<string>? error)> GetTokenWithClientCredentials()
        {
            //Tuple<bool,string,string>
            //  token,true,null
            //  null,false,error


            if (_memoryCache.TryGetValue(TokenKey, out string? token))
            {
                return (true, token!, null);
            }


            var requestAsBody =
                new ClientCredentialTokenRequestDto(_tokenOptions.Value.ClientId, _tokenOptions.Value.ClientSecret);

            var response =
                await _httpClient.PostAsJsonAsync("/api/Token/CreateClientCredential",
                    requestAsBody);


            var responseAsBody =
                await response.Content.ReadFromJsonAsync<ResponseModelDto<ClientCredentialTokenResponseDto>>();


            if (!response.IsSuccessStatusCode)
            {
                return (false, null, responseAsBody!.FailMessages);
            }


            _memoryCache.Set(TokenKey, responseAsBody!.Data!.AccessToken, TimeSpan.FromHours(9));

            return (true, responseAsBody.Data.AccessToken, null);
        }


        public void ClearTokenCache()
        {
            _memoryCache.Remove(TokenKey);
        }
    }
}
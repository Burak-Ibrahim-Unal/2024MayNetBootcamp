using System.Net;
using Bootcamp.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Bootcamp.Web.TokenServices
{
    public class ClientCredentialTokenInterceptor(TokenService _tokenService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var tokenAsClientCredential = await _tokenService.GetTokenWithClientCredentials();

            if (!tokenAsClientCredential.isSuccess)
            {
                throw new Exception("Token Servisinde hata var");
            }

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenAsClientCredential.token);


            var response = await base.SendAsync(request, cancellationToken);


            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //  throw new UnauthorizedAccessException();

                _tokenService.ClearTokenCache();


                tokenAsClientCredential = await _tokenService.GetTokenWithClientCredentials();

                if (!tokenAsClientCredential.isSuccess)
                {
                    throw new Exception("Token Servisinde hata var");
                }

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenAsClientCredential.token);

                response = await base.SendAsync(request, cancellationToken);
            }


            return response;
        }
    }
}
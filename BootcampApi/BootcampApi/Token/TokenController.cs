using Bootcamp.Service.Token;
using BootcampApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.Api.Token
{
    public class TokenController(ITokenService _tokenService) : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateClientToken(GetAccessTokenRequestDto request)
        {
            var response = await _tokenService.CreateClientAccessToken(request);
            return CreateActionResult(response);
        }
    }
}
using Bootcamp.Service.SharedDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Service.Token
{
    public interface ITokenService
    {
        Task<ResponseModelDto<TokenResponseDto>> CreateClientAccessToken(GetAccessTokenRequestDto request);
    }
}

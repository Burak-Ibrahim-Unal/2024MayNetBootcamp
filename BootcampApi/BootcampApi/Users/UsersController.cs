using Bootcamp.Service.Users;
using BootcampApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.Api.Users
{
    public class UsersController(UserService _userService) : CustomBaseController
    {
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequestDto request)
        {
            return CreateActionResult(await _userService.SignUp(request));
        }


        //signin
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequestDto request)
        {
            return CreateActionResult(await _userService.SignIn(request));
        }

        #region UNCOMMENT IF NEEDED
        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var response = await _userService.GetUsers();

        //    return Ok(response);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUserById(int id)
        //{
        //    var response = await _userService.GetUserById(id);

        //    return Ok(response);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, UpdateUserRequestDto updateUserRequestDto)
        //{
        //    var response = await _userService.UpdateUser(id, updateUserRequestDto);

        //    return Ok(response);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    var response = await _userService.DeleteUser(id);

        //    return Ok(response);
        //} 
        #endregion
    }
}

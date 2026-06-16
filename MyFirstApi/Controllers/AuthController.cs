using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MyFirstApi.Dto;
using MyFirstApi.Generic_Collections;
using MyFirstApi.IService;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            try
            {
                var result = await _authService.LoginUser(userDto);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }
                if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }
                if (result.Item1 == 2)
                {
                    return Ok(ResponseResult<TokenDto>.Success(result.Item2, result.Item2.Message));
                }
                return StatusCode(500, "Unexpected result code.");
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDto Dto)
        {
            try
            {
                var result = await _authService.RegisterUser(Dto);

                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch
            {
                throw;
            }

        }


    }
}










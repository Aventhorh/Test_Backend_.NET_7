using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Test_Backend_NET_7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserById(int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(UserDto request)
        {
            return Ok(await _userService.Register(request));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<UserLoginDto>>> Login(UserDto request)
        {
            return Ok(await _userService.Login(request));
        }

        [HttpDelete("{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(int id)
        {
            return Ok(await _userService.Delete(id));
        }

        [HttpPut("{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult<ServiceResponse<UserUpdateDto>>> Update(int id, UserDto request)
        {
            return Ok(await _userService.Update(id, request));
        }
    }
}
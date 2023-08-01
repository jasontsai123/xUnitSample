using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using xUnitSample.Controllers.Models;
using xUnitSample.Infrastructure.Helpers;
using xUnitSample.Service;
using xUnitSample.Service.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace xUnitSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserService _userService;

        public UserController(IHttpContextAccessor httpContextAccessor, IJwtHelper jwtHelper, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Id, request.Password);
            if (user.IsAuthenticated == false)
            {
                return Unauthorized();
            }

            var token = _jwtHelper.GenerateJwtToken(request.Id);
            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var userExist = (await _userService.GetAsync(request.Id))?.Id == request.Id;
            if (userExist)
            {
                return Ok(new { Succeeded = false, Message = "帳號已存在!" });
            }

            var isCreatedSuccessfully = await _userService.CreateAsync(new CreateUserDto
            {
                Id = request.Id,
                Name = request.Name,
                Password = request.Password
            });
            if (isCreatedSuccessfully == false)
            {
                return Ok(new { Succeeded = false, Message = "註冊失敗!" });
            }

            return Ok(new { Succeeded = true, Message = "已註冊完成!" });
        }

        [Authorize]
        [HttpGet("Information")]
        public async Task<IActionResult> Get()
        {
            var id = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var result = await _userService.GetAsync(id);

            return Ok(result);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
    {
        var result = await _userService.RegisterUserAsync(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        var result = await _userService.AuthenticateAsync(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return Unauthorized(result.Errors);
    }
}

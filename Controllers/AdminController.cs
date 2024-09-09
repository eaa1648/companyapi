using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AdminRegistrationModel model)
    {
        var result = await _adminService.RegisterAdminAsync(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginModel model)
    {
        var result = await _adminService.AuthenticateAsync(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return Unauthorized(result.Errors);
    }
   [HttpPost("create-sub-admin")]
public async Task<IActionResult> CreateSubAdmin([FromBody] CreateSubAdminModel model)
{
    var result = await _adminService.CreateSubAdminAsync(model);
    if (result.IsSuccess)
    {
        return Ok(result);
    }
    return BadRequest(result.Errors);
}



    [HttpGet("users")]
    public async Task<IActionResult> ListUsers()
    {
        var result = await _adminService.ListUsersAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("users/{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var result = await _adminService.DeleteUserAsync(username);
        if (result.IsSuccess)
        {
            return NoContent(); // 204 No Content
        }
        return BadRequest(result.Errors);
    }
}

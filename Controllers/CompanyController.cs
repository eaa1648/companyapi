using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Company")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _companyService;

    public CompanyController(CompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost("create-subuser")]
    public async Task<IActionResult> CreateSubUser([FromBody] CreateSubUserModel model)
    {
        var result = await _companyService.CreateSubUserAsync(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Errors);
    }
}

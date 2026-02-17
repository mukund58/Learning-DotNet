using Microsoft.Extensions.Configuration; // Ensure this is present
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]


public class UsersController : ControllerBase
{

    // private readonly IConfiguration _configuration; // not working

    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

    public UsersController(Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [HttpGet("appinfo")]
    public IActionResult GetAppInfo()
    {
        // Returns a string by default

        var name = _configuration["MyApp:Name"];
        var version = _configuration["MyApp:Version"];

        return Ok(new
        {
            name,
            version
        });
    }

}
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")] // map to api/users 
public class UsersController : ControllerBase
{

    [HttpGet]  // api/users/
    public IActionResult Hello(){
        return Ok("Hello from Users");
    }
    [HttpGet("hello")] // api/users/hello 
    public IActionResult HelloController(){
        return Ok("Hello from UsersController");
    }
}


using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        return Ok($"Requested user id: {id}");
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] UserDto dto)
    {
        return Ok($"User {dto.Name} created");
    }
}

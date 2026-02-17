using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/[controller]")]

public class UsersController(IMessageService messageService) : ControllerBase // constructor + class declaration
{
    [HttpGet("msg")]
    public IActionResult GetMessage()
    {
        return Ok(messageService.GetMessage());
    }


}
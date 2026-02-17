using Microsoft.AspNetCore.Mvc;
using System;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMessageService _messageService;

    public UsersController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("message")]
    public IActionResult GetMessage()
    {
        return Ok(_messageService.GetMessage());
    }
}

using Core.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCoursesWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IAccountsService accountsService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        await accountsService.Register(model);
        return Ok();
    }
}

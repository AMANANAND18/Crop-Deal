using CropDeal.Web.Models;
using CropDeal.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CropDeal.Web.Controllers;

[Route("account")]
public class AccountController(IAuthService authService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var result = await authService.RegisterAsync(model.FullName, model.Email, model.Password, model.Role);
        return result.Success ? Ok(result.Message) : BadRequest(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        var user = await authService.LoginAsync(model.Email, model.Password);
        if (user is null)
            return Unauthorized("Invalid credentials or inactive user.");

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            Role = user.Role.ToString()
        });
    }
}

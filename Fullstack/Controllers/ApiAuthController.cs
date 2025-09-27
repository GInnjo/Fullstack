using Fullstack.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiAuthController : ControllerBase
{
    private readonly TokenStorage _tokenStore;

    public ApiAuthController(TokenStorage tokenStore)
    {
        _tokenStore = tokenStore;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginViewModel form)
    {
        Console.WriteLine("Login attempt to gain a token!");
        if (!ModelState.IsValid)
            return BadRequest("Invalid data");

        var user = DatabaseHandler.GetUserByEmail(form.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password" });

        var password = DatabaseHandler.GetById<Password>(user.Id);
        if (!password.VerifyPassword(form.Password))
            return Unauthorized(new { message = "Invalid email or password" });

        // ✅ Generate token
        var token = _tokenStore.GenerateToken(user.Id.ToString());

        // Optionally update last login timestamp
        user.LastWebLogin = DateTime.UtcNow;
        DatabaseHandler.Save(user);

        return Ok(new { token });
    }
}
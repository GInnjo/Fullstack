using Fullstack.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fullstack.Controllers;

[Authorize(Policy = "DenyAuthenticated")]
public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TryLogin(LoginViewModel form)
    {
        if (ModelState.IsValid)
        {
            User user = DatabaseHandler.GetUserByEmail(form.Email);
            if (user != null)
            {
                Password password = DatabaseHandler.GetById<Password>(user.PasswordId);
                if (password.VerifyPassword(form.Password))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email)
                };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password");
            }

        }

        return View("Index", form);
    }
}
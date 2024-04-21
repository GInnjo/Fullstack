using Fullstack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.Controllers;

[Authorize(Policy = "DenyAuthenticated")]
public class RegisterController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TryRegister(RegisterViewModel form)
    {
        if (ModelState.IsValid)
        {
            Password password = new Password(form.Password);
            DatabaseHandler.Save(password);
            Storage storage = new Storage();
            DatabaseHandler.Save(storage);

            User user = new User
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                CreatedAt = DateTime.Now,
                Role = "User",
                PasswordId = password.Id,
                StorageId = storage.Id,
                Username = form.Username
            };
            DatabaseHandler.Save(user);

            return RedirectToAction("Index", "Login");
        }
        return View("Index", form);
    }
}

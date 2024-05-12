using Fullstack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
			User userWithSameEmail = DatabaseHandler.GetUserByEmail(form.Email);

            if (userWithSameEmail != null)
            {
				ModelState.AddModelError("", "Email already in use");
			}

			User userWithSameUsername = DatabaseHandler.GetUserByUsername(form.Username);

			if (userWithSameUsername != null)
            {
				ModelState.AddModelError("", "Username already in use");
			}

			if (userWithSameEmail != null || userWithSameUsername != null)
            {
				return View("Index", form);
			}
           

            User user = new User
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                CreatedAt = DateTime.Now,
                Role = "User",
                Username = form.Username
            };
            DatabaseHandler.Save(user);

            Password password = new Password(user.Id, form.Password);
            DatabaseHandler.Save(password);
            Storage storage = new Storage(user.Id);
            DatabaseHandler.Save(storage);

            GameInstance gameInstance = new GameInstance
            {
                Id = user.Id,
                Name = user.Username + "'s Island",
                CreatedAt = DateTime.Now,
                Status = "Active",
                fishMap = new FishMap()
            };
            gameInstance.InvitedPlayerIds.Add(user.Id);
            gameInstance.fishMap.InitFishMap();
            DatabaseHandler.Save(gameInstance);

            return RedirectToAction("Index", "Login");
        }
        return View("Index", form);
    }
}

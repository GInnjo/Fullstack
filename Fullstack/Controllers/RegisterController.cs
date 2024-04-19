using Fullstack.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.Controllers
{
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
                    StorageId = storage.Id
                };
                DatabaseHandler.Save(user);

                DatabaseHandler.Delete<User>(user.Id);
            }
            return View("Index", form);
        }
    }
}

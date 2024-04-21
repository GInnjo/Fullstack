using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fullstack.Models;
using System.Security.Claims;
using MongoDB.Bson;
using System.IO.Pipes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Fullstack.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            User user = DatabaseHandler.GetById<User>(ObjectId.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            EditViewModel editViewModel = new EditViewModel
            {
				UserForm = new UserEditViewModel
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Username = user.Username
				},
				PasswordForm = new PasswordEditViewModel()
			};
            return View(editViewModel);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> TryEditUser([Bind(Prefix = "UserForm")] UserEditViewModel form)
		{
			if (ModelState.IsValid)
			{
				User user = DatabaseHandler.GetById<User>(ObjectId.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
				user.FirstName = form.FirstName;
				user.LastName = form.LastName;
				user.Email = form.Email;
				user.Username = form.Username;
				DatabaseHandler.Save(user);
			}

			EditViewModel model = new EditViewModel
			{
				UserForm = form,
				PasswordForm = new PasswordEditViewModel()
			};

			return View("Index", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> TryEditPassword([Bind(Prefix = "PasswordForm")] PasswordEditViewModel form)
		{
			User user = DatabaseHandler.GetById<User>(ObjectId.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

			if (ModelState.IsValid)
			{
				if (form.Password == form.ConfirmPassword)
				{
					Password password = DatabaseHandler.GetById<Password>(user.PasswordId);
					if (password.VerifyPassword(form.CurrentPassword))
					{
						Password dummy_psw = new Password(form.Password);
						password.hashedPassword = dummy_psw.hashedPassword;
						password.salt = dummy_psw.salt;
						DatabaseHandler.Save(password);
					}
					else
					{
						ModelState.AddModelError("", "Current password is incorrect.");
					}
				}
				else 
				{ 
					ModelState.AddModelError("", "Passwords do not match.");
				}
			}

			EditViewModel model = new EditViewModel
			{
				UserForm = new UserEditViewModel
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Username = user.Username
				},
				PasswordForm = form
			};

			return View("Index", model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteAccount()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			User user = DatabaseHandler.GetById<User>(ObjectId.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
			DatabaseHandler.Delete<User>(user.Id);

			return RedirectToAction("Index", "Home");
		}
	}


}

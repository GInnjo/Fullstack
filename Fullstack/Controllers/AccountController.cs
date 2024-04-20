using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fullstack.Models;
using System.Security.Claims;
using MongoDB.Bson;

namespace Fullstack.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            User user = DatabaseHandler.GetById<User>(ObjectId.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            return View(user);
        }
    }
}

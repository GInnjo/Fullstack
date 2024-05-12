using Fullstack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;
using System.Text.Json;

namespace Fullstack.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class IslandsController : Controller
	{
		public IActionResult Index()
		{
			ObjectId id = ObjectId.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            GameInstance usersGame = DatabaseHandler.GetById<GameInstance>(id);
			usersGame.fishMap.PopulateFishArray();

            return View(usersGame.fishMap.fishes);
		}
	}
}

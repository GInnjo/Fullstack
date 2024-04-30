using Microsoft.AspNetCore.Mvc;

namespace Fullstack.Controllers
{
	public class IslandsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

using Microsoft.AspNetCore.Mvc;

namespace Fullstack.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

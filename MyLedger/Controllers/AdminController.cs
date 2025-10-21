using Microsoft.AspNetCore.Mvc;

namespace MyLedger.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

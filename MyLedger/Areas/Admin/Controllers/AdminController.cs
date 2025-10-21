using Microsoft.AspNetCore.Mvc;

namespace MyLedger.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Route("Admin")]
	public class AdminController : Controller
    {
		[Route("")]
		public IActionResult Index()
        {
            return View();
        }
    }
}

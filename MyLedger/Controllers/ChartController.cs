using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace MyLedger.Controllers
{
	public class ChartController : Controller
	{
		Context c = new Context();
		[HttpGet]
		public IActionResult Index()
		{

			c.Transactions.Select(x => x.Year).ToList();

			List<SelectListItem> yearValues = c.Transactions
												.Select(x => x.Year)
												.Distinct()
												.OrderByDescending(y => y)
												.Select(y => new SelectListItem
												{
													Text = y.ToString(),
													Value = y.ToString()
												}).ToList();

			ViewBag.yearValues = yearValues;
			ViewBag.defaultYear = DateTime.Now.Year;
			return View();

		}
		[HttpGet]
		public JsonResult GetTransactionsByYear(int year)
		{
			var userName = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
			var gelir = new decimal[12];
			var gider = new decimal[12];

			var data = c.Transactions
				.Where(t => t.Year == year && t.CreatedBy==userId)
				.ToList();
			
			foreach (var item in data)
			{
				int monthIndex = item.Month - 1;
				if (monthIndex < 0 || monthIndex > 11) continue;

				var type = (item.Type ?? "").Trim();

				if (type == "Income")
				{
					gelir[monthIndex] += item.Amount;
				}
				else if (type == "Expense")
				{
					gider[monthIndex] += item.Amount;
				}
			}

			return Json(new { gelir, gider });
		}
	}
}

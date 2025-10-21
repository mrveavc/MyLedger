using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyLedger.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {   
            Context c=new Context();
            TransactionManager tm = new TransactionManager(new EfTransactionRepository());
            var values = tm.GetTransactionListWithUser();
			List<SelectListItem> transactionValues = values.Select(y => new SelectListItem
												{
													Text = y.User.FullName.ToString(),
													Value = y.User.Id.ToString()
												}).ToList();
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
            ViewBag.transactionValues = transactionValues;

			return View();
        }
        [HttpGet]
        public JsonResult GetTransactionsByYear(int year, int User)
        {
            Context c = new Context();
            var gelir = new decimal[12];
            var gider = new decimal[12];

            var data = c.Transactions
                .Where(t => t.Year == year && t.CreatedBy == User)
                .ToList();

            foreach (var item in data)
            {
                int monthIndex = item.Month - 1;
                if (monthIndex < 0 || monthIndex > 11) continue;

                var type = (item.Type ?? "").Trim();

                if (type == "Income")
                    gelir[monthIndex] += item.Amount;
                else if (type == "Expense")
                    gider[monthIndex] += item.Amount;
            }

            return Json(new { gelir, gider });
        }
    }
}

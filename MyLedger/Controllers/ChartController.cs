using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLedger.Models;



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
        [HttpGet]
        public JsonResult GetTransactionsByYearAndLedger(int year)
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();

            var ledgerIds = c.LedgerMembers
                  .Where(x => x.UserId == userId)
                  .Select(x => x.LedgerId)
                  .ToList();

            var data = c.Transactions
                        .Include(t => t.Ledger)
                        .Where(t => t.LedgerId.HasValue && ledgerIds.Contains(t.LedgerId.Value) && t.Year == year)
                        .ToList();

            // Her defter için gelir ve gider dizisi oluştur
            var result = new List<object>();
            foreach (var ledgerId in ledgerIds)
            {
                var ledgerName = c.Ledgers.Where(l => l.Id == ledgerId).Select(l => l.Name).FirstOrDefault();

                var gelir = new decimal[12];
                var gider = new decimal[12];

                var ledgerTransactions = data.Where(t => t.LedgerId == ledgerId).ToList();

                foreach (var item in ledgerTransactions)
                {
                    int monthIndex = item.Month - 1;
                    if (monthIndex < 0 || monthIndex > 11) continue;

                    var type = (item.Type ?? "").Trim();
                    if (type == "Income") gelir[monthIndex] += item.Amount;
                    else if (type == "Expense") gider[monthIndex] += item.Amount;
                }

                result.Add(new
                {
                    ledgerName,
                    gelir,
                    gider
                });
            }

            return Json(result);
        }
        public IActionResult ExpenseChart() {

            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();

            if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
            {
                var transactions = c.Transactions
                .Where(t => t.Type == "Expense" && t.CreatedBy == userId)
                .GroupBy(t => t.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToList();

                var totalExpense = transactions.Sum(t => t.TotalAmount);

                var categoryList = transactions.Select(t => new CategoryClass
                {
                    categoryname = t.Category,
                    categorytotal = t.TotalAmount,
                    percentage = (t.TotalAmount / totalExpense) * 100
                }).ToList();

                // Veriyi ViewBag üzerinden gönderiyoruz
                ViewBag.CategoryList = categoryList;
            }
            else if (User.IsInRole("Member")){
                var ledgerIds = c.LedgerMembers
             .Where(x => x.UserId == userId)
             .Select(x => x.LedgerId)
             .ToList();

                var data = new List<object>();

                foreach (var ledgerId in ledgerIds)
                {
                    var ledgerName = c.Ledgers.Where(l => l.Id == ledgerId).Select(l => l.Name).FirstOrDefault();

                    var transactions = c.Transactions
                        .Where(t => t.LedgerId == ledgerId && t.Type == "Expense" )
                        .GroupBy(t => t.Category)
                        .Select(g => new
                        {
                            Category = g.Key,
                            TotalAmount = g.Sum(t => t.Amount)
                        })
                        .ToList();

                    var totalExpense = transactions.Sum(t => t.TotalAmount);

                    var categoryList = transactions.Select(t => new CategoryClass
                    {
                        categoryname = t.Category,
                        categorytotal = t.TotalAmount,
                        percentage = totalExpense > 0 ? (t.TotalAmount / totalExpense) * 100 : 0
                    }).ToList();

                    data.Add(new
                    {
                        ledgerName,
                        categories = categoryList
                    });
                }

                ViewBag.MemberLedgerExpenses = data;
            }

            return View();
        }
	}
}

using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyLedger.Controllers
{
    public class DashboardController : Controller
    {
        Context c=new Context();
        BankManager bm = new BankManager(new EfBankRepository());
        public async  Task<IActionResult> Index()
        {
            TransactionManager tm=new TransactionManager(new EfTransactionRepository());
            decimal TotalIncome =0;
            decimal TotalExpense = 0;
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var incomeResult = c.Transactions
                          .Where(x => x.CreatedBy == userId && x.Type == "Income")
                          .ToList();
            foreach (var transaction in incomeResult)
            {
                TotalIncome += transaction.Amount;
            }
            ViewBag.TotalIncome = TotalIncome;
            var expenseResult = c.Transactions
                          .Where(x => x.CreatedBy == userId && x.Type == "Expense")
                          .ToList();
            foreach (var transaction in expenseResult)
            {
                TotalExpense += transaction.Amount;
            }
            ViewBag.TotalExpense = TotalExpense;

            var netBalance = TotalIncome - TotalExpense;
            ViewBag.NetBalance = netBalance;
            //var bank = c.Transactions
            //              .Where(x => x.CreatedBy == userId).Select(y=>y.BankId)
            //              .ToList();
            //var values = tm.GetTransactionListWithBank(userId);

            var valuess = bm.GetBankListWithLedger(userId);
            var countBank = valuess.Count();
            ViewBag.countBank = countBank;

            var lastTransactions = await c.Transactions.Where(x =>x.CreatedBy == userId).OrderByDescending(x => x.CreatedAt).Take(5).Select(x => new
            {
                x.CreatedAt,
                x.Description,
                x.Amount
            })
                .ToListAsync();
            ViewBag.LastTransactions = lastTransactions;

            //Şuanki aydan 6 ay öncesi

            var lastMonths = Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-i))
                .OrderBy(x => x)
                .ToList();

            var months = new List<string>();
            var monthlyIncome = new List<decimal>();
            var monthlyExpense = new List<decimal>();

            foreach (var m in lastMonths)
            {
                var start = new DateTime(m.Year, m.Month, 1);
                var end = start.AddMonths(1);

                var income = await c.Transactions
                    .Where(x => x.CreatedBy == userId &&  x.Type == "Income" && x.CreatedAt >= start && x.CreatedAt < end)
                    .SumAsync(x => (decimal?)x.Amount) ?? 0;

                var expense = await c.Transactions
                    .Where(x => x.CreatedBy == userId && x.Type == "Expense" && x.CreatedAt >= start && x.CreatedAt < end)
                    .SumAsync(x => (decimal?)x.Amount) ?? 0;

                months.Add(m.ToString("MMM yyyy")); // Örn: "Ağu 2025"
                monthlyIncome.Add(income);
                monthlyExpense.Add(expense);
            }
            ViewBag.Months = months;
            ViewBag.MonthlyIncome = monthlyIncome;
            ViewBag.MonthlyExpense = monthlyExpense;


            return View();
            //return View(values);
        }
    }
}

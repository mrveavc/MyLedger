using System.Globalization;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLedger.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyLedger.Controllers
{
	public class TransactionController : Controller
	{
		Context c=new Context();
		TransactionManager tm = new TransactionManager(new EfTransactionRepository());
		LedgerManager lm = new LedgerManager(new EfLedgerRepository());

		public IActionResult Index()
		{
			
			var userName = User.Identity.Name;
			var userId=c.Users.Where(x=>x.UserName==userName).Select(y=>y.Id).FirstOrDefault();
            //var transaction = c.Transactions.Where(x => x.CreatedBy == userId).ToList();
            if (User.IsInRole("Admin") || User.IsInRole("Accountant"))
            {
                 var values = tm.GetTransactionListWithLedgerBank(userId);
			//	var ownerName = c.Users
			//	   .Where(x => x.Id == userId)
			//	   .Select(x => x.FullName)
			//	   .FirstOrDefault();

			//ViewBag.OwnerName = ownerName;
			return View(values);
            }
            else if (User.IsInRole("Member"))
            {
                // Kullanıcının üye olduğu Ledger Id'lerini bul
                var ledgerIds = c.LedgerMembers
                    .Where(x => x.UserId == userId)
                    .Select(x => x.LedgerId)
                    .ToList();

                // O Ledger'lara ait Transaction'ları getir
                var transactions = c.Transactions
                    .Include(t => t.Ledger)
                    .Include(t => t.Bank)
					.Include(t=>t.User)
                    .Where(t => t.LedgerId.HasValue && ledgerIds.Contains(t.LedgerId.Value))
                    .ToList();
                // İstersen ViewBag ile isimleri aktarabilirsin
                // Örn: ilk sahibin adını ViewBag’e
              
                return View(transactions);

            }
            return View(new List<Transaction>());
        }
        [HttpGet]
		public IActionResult AddTransaction()
		{
			var username = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
			var ownerName = c.Users
				   .Where(x => x.Id == userId)
				   .Select(x => x.FullName)
				   .FirstOrDefault();
			ViewBag.OwnerId = userId;
			ViewBag.OwnerName = ownerName;
			//Defter
			var ledgerValues = c.Ledgers.Where(x => x.UserId == userId).ToList();
			List<SelectListItem> ledgervalues = ledgerValues.Select(x => new SelectListItem
			{
				Text = x.Name.ToString(),
				Value = x.Id.ToString()
			})
				.ToList();
			ViewBag.lv = ledgervalues;
			//Banka
			List<SelectListItem> bankvalues = c.Banks.Select(x => new SelectListItem
			{
				Text = x.BankName.ToString(),
				Value = x.Id.ToString()
			})
				.ToList();
			ViewBag.bank = bankvalues;

			ViewBag.TypeList = Enum.GetValues(typeof(TransactionType))
								  .Cast<TransactionType>()
								  .Select(x => new SelectListItem
								  {
									  Text = x.ToString(),
									  Value = x.ToString()
								  })
								  .ToList();

			var months = Enumerable.Range(1, 12)
								   .Select(i => new SelectListItem
								   {
									  Value = i.ToString(),  
									  Text = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(i) 
									})
								   .ToList();

			ViewBag.months = months;


			var incomeCategories = Enum.GetValues(typeof(IncomeCategory))
		.Cast<IncomeCategory>()
		.Select(c => new { Value = c.ToString(), Text = c.ToString() })
		.ToList();

			var expenseCategories = Enum.GetValues(typeof(ExpenseCategory))
				.Cast<ExpenseCategory>()
				.Select(c => new { Value = c.ToString(), Text = c.ToString() })
				.ToList();

			ViewBag.IncomeCategoriesJson = JsonConvert.SerializeObject(incomeCategories);
			ViewBag.ExpenseCategoriesJson = JsonConvert.SerializeObject(expenseCategories);
			return View();
		}
		[HttpPost]
		public IActionResult AddTransaction(Transaction p, string OtherCategory)
		{
			if(p.Category == "Diğer" && !string.IsNullOrWhiteSpace(OtherCategory))

	{
				// "Diğer" seçildiyse kullanıcıdan gelen değeri al
				p.Category = OtherCategory;
			}
			tm.TAdd(p);
			return RedirectToAction("Index", "Transaction");
		}
		[HttpGet]
		public IActionResult EditTransaction(int id)
		{
			var username = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
			var transactionValue = tm.TGetById(id);
			var ownerName = c.Users
				   .Where(x => x.Id == userId)
				   .Select(x => x.FullName)
				   .FirstOrDefault();
			ViewBag.OwnerId = userId;
			ViewBag.OwnerName = ownerName;
			var ledgerValues = c.Ledgers.Where(x => x.UserId == userId).ToList();
			List<SelectListItem> ledgervalues = ledgerValues.Select(x => new SelectListItem
			{
				Text = x.Name.ToString(),
				Value = x.Id.ToString()
			})
				.ToList();
			ViewBag.lv = ledgervalues;

			List<SelectListItem> bankvalues = c.Banks.Select(x => new SelectListItem
			{
				Text = x.BankName.ToString(),
				Value = x.Id.ToString()
			})
				.ToList();
			ViewBag.bank = bankvalues;


			ViewBag.TypeList = Enum.GetValues(typeof(TransactionType))
								  .Cast<TransactionType>()
								  .Select(x => new SelectListItem
								  {
									  Text = x.ToString(),
									  Value = x.ToString()
								  })
								  .ToList();
			var months = Enumerable.Range(1, 12)
								   .Select(i => new SelectListItem
								   {
									   Value = i.ToString(),
									   Text = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(i)
								   })
								   .ToList();

			ViewBag.months = months;
			return View(transactionValue);
		}
		[HttpPost]	
		public IActionResult EditTransaction(Transaction p)
		{
			tm.TUpdate(p);
			return RedirectToAction("Index");
		}
		public IActionResult DeleteTransaction(int id)
		{
			var value=tm.TGetById(id);
			tm.TDelete(value);
			return RedirectToAction("Index");

		}
	}
}

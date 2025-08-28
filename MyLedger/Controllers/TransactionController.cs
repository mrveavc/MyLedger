using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace MyLedger.Controllers
{
	public class TransactionController : Controller
	{
		Context c=new Context();
		TransactionManager tm = new TransactionManager(new EfTransactionRepository());
		public IActionResult Index()
		{
			var userName = User.Identity.Name;
			var userId=c.Users.Where(x=>x.UserName==userName).Select(y=>y.Id).FirstOrDefault();
			//var transaction = c.Transactions.Where(x => x.CreatedBy == userId).ToList();
			var values = tm.GetTransactionListWithLedgerBank(userId);
			var ownerName = c.Users
				   .Where(x => x.Id == userId)
				   .Select(x => x.FullName)
				   .FirstOrDefault();

			ViewBag.OwnerName = ownerName;
			return View(values);
		}
	}
}

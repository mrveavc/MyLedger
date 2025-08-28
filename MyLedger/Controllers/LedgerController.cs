using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyLedger.Controllers
{
	public class LedgerController : Controller
	{
		Context c = new Context();
		LedgerManager lm = new LedgerManager(new EfLedgerRepository());
		public IActionResult Index()
		{
			var username = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
			var values = lm.GetListWithLedgerName(userId);
			var ownerName = c.Users
				   .Where(x => x.Id == userId)
				   .Select(x => x.FullName)
				   .FirstOrDefault();

			ViewBag.OwnerName = ownerName;
			return View(values);
		}
		public IActionResult DeleteLedger(int id)
		{
			var values = lm.TGetById(id);
			lm.TDelete(values);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult AddLedger()
		{
			var username = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
			var ownerName = c.Users
				   .Where(x => x.Id == userId)
				   .Select(x => x.FullName)
				   .FirstOrDefault();

			ViewBag.OwnerName = ownerName;


			return View();
		}
		[HttpPost]
		public IActionResult AddLedger(Ledger p)
		{
			var username = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
			p.UserId = userId;
			p.CreatedAt = DateTime.Now;
			lm.TAdd(p);

			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult EditLedger(int id)
		{
			var ledgervalue = lm.TGetById(id);
			var ownerName = c.Users
		.Where(x => x.Id == ledgervalue.UserId)
		.Select(x => x.FullName)
		.FirstOrDefault();

			ViewBag.OwnerName = ownerName;

			return View(ledgervalue);
		}
		[HttpPost]
		public IActionResult EditLedger(Ledger p)
		{
			var ledgervalue = lm.TGetById(p.Id);
			if (ledgervalue != null)
			{
				ledgervalue.Name = p.Name;

				lm.TUpdate(ledgervalue);
			}


			return RedirectToAction("Index");
		}
		public IActionResult MyLedgers()
		{
			var username = User.Identity.Name;
			var userId = c.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();

			var ledgerList = c.LedgerMembers
                      .Where(x => x.UserId == userId)
                      .Include(x => x.Ledger) 
                      .Select(x => x.Ledger) 
                      .ToList();
            var ownerIds = ledgerList.Select(l => l.UserId).Distinct().ToList();
            var owners = c.Users.Where(u => ownerIds.Contains(u.Id))
                                .ToDictionary(u => u.Id, u => u.FullName);
            ViewBag.Owners = owners;
            return View(ledgerList);
		}

	}
}

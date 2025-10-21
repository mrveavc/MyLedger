using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace MyLedger.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LedgerController : Controller
    {
        Context c = new Context();
        LedgerManager lm = new LedgerManager(new EfLedgerRepository());
        public IActionResult Index()
        {
            var values = lm.GetListWithUserName();

            return View(values);
        }
        public IActionResult DeleteLedger(int id)
        {
            var values = lm.TGetById(id);
            lm.TDelete(values);
            return RedirectToAction("Index", "Ledger", new { area = "Admin" });
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
    }
}

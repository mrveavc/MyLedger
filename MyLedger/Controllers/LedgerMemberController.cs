using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyLedger.Controllers
{
    public class LedgerMemberController : Controller
    {
        Context c = new Context();
        LedgerMemberManager mm= new LedgerMemberManager(new EfLedgerMemberRepository());
        LedgerManager lm = new LedgerManager(new EfLedgerRepository());
        UserManager um= new UserManager(new EfUserRepository());


        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();

            var values=mm.GetLedgerMemberListWithLedgerUser(userId);
            return View(values);
        }
        [HttpGet]
        public IActionResult AddMember()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var values = lm.GetListWithLedgerName(userId);

            var userValues=c.Users
                .Where(x => x.Id != userId).ToList();

            List<SelectListItem> ledgervalues = values.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
                .ToList();
            List<SelectListItem> uservalues = userValues.Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.Id.ToString()
            })
               .ToList();


            ViewBag.lv = ledgervalues;
            ViewBag.uv=uservalues;
            return View();

        }
        [HttpPost]
        public IActionResult AddMember(LedgerMember p)
        {
            mm.TAdd(p);
            return RedirectToAction("Index", "LedgerMember");

        }
    }
}

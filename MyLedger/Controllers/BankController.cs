using System.Collections.Generic;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyLedger.Controllers
{
    public class BankController : Controller
    {
        BankManager bm=new BankManager(new EfBankRepository());
        Context c=new Context();
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            //var ledger=c.Ledgers.Where(x=>x.UserId==userId).Select(y=>y.Id).ToList();
            var values = bm.GetBankListWithLedger(userId);

            return View(values);
        }
        [HttpGet]
        public IActionResult AddBank()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            //var ledger=c.Ledgers.Where(x=>x.UserId==userId).Select(y=>y.Id).ToList();
            var values = bm.GetBankListWithLedger(userId);
         
            List<SelectListItem> ledgervalues = values //aynı olan ledger nameleri getirmiyor.
                .DistinctBy(x => x.Ledger.Id)
                .Select(x => new SelectListItem
                {
                    Text = x.Ledger.Name,
                    Value = x.Ledger.Id.ToString()
                })
                .ToList();
            ViewBag.lv = ledgervalues;
            return View();
        }
        [HttpPost]
        public IActionResult AddBank(Bank p)
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var values = bm.GetBankListWithLedger(userId);
            BankValidator bv=new BankValidator();
            ValidationResult result=bv.Validate(p);
           
            if (result.IsValid)
            {
                p.AccountNumber = "TR" + p.AccountNumber;
                bm.TAdd(p);
                return RedirectToAction("Index", "Bank");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditBank(int id) { 
            var bankvalue=bm.TGetById(id);
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            //var ledger=c.Ledgers.Where(x=>x.UserId==userId).Select(y=>y.Id).ToList();
            var values = bm.GetBankListWithLedger(userId);

            List<SelectListItem> ledgervalues = values //aynı olan ledger nameleri getirmiyor.
                .DistinctBy(x => x.Ledger.Id)
                .Select(x => new SelectListItem
                {
                    Text = x.Ledger.Name,
                    Value = x.Ledger.Id.ToString()
                })
                .ToList();
            ViewBag.lv = ledgervalues;
            return View(bankvalue);
        }
        [HttpPost]
        public IActionResult EditBank(Bank p)
        {
            var bankvalue= bm.TGetById(p.Id);
            bankvalue.Id = p.Id;
            bankvalue.AccountNumber = p.AccountNumber;
            bankvalue.BankName = p.BankName;
            bankvalue.Balance = p.Balance;
            bankvalue.LedgerId = p.LedgerId;
            bm.TUpdate(p);
            return RedirectToAction("Index"); 
        }
        public IActionResult DeleteBank(int id)
        {
            var bankvalue=bm.TGetById(id);
            bm.TDelete(bankvalue);
            return RedirectToAction("Index");
        }
    }
}

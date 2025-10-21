using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace MyLedger.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BankController : Controller
	{
		Context c = new Context();
		BankManager bm = new BankManager(new EfBankRepository());

		public IActionResult Index()
		{
			var values = bm.GetList();
			return View(values);
		}
		public IActionResult DeleteBank(int id)
		{
			var bankValues = bm.TGetById(id);
			bm.TDelete(bankValues);


			return RedirectToAction("Index", "Bank", new { area = "Admin" });
		}
		[HttpGet]
		public IActionResult EditBank(int id)
		{
			var bankValues= bm.TGetById(id);
            bankValues.AccountNumber = bankValues.AccountNumber.Substring(2);

            return View(bankValues);

		}
		[HttpPost]
		public IActionResult Editbank(Bank p)
		{
			var bankValues = bm.TGetById(p.Id);
            BankValidator bv = new BankValidator();
            //var IbanWithoutTR = p.AccountNumber.Substring(2);
            ValidationResult result = bv.Validate(p);
            if (result.IsValid)
            {
                bankValues.Id = p.Id;
                bankValues.AccountNumber = p.AccountNumber;
                bankValues.BankName = p.BankName;
                bankValues.Balance = p.Balance;
                p.AccountNumber = "TR" + p.AccountNumber;
                //bankvalue.LedgerId = p.LedgerId;
                bm.TUpdate(p);
                return RedirectToAction("Index");
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
	}
}

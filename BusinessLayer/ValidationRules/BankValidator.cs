using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using FluentValidation;
namespace BusinessLayer.ValidationRules
{
    public class BankValidator: AbstractValidator<Bank>
    {
        public BankValidator()
        {
            RuleFor(x => x.AccountNumber).Length(24).WithMessage("Başına TR koymadan 24 basamaklı sayı giriniz");
        }
    }
}

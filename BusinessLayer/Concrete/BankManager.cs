using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class BankManager : IBankService
    {
        IBankDal _bankDal;

        public BankManager(IBankDal bankDal)
        {
            _bankDal = bankDal;
        }

        public List<Bank> GetBankListWithLedger(int id)
        {
           return  _bankDal.GetListWithLedger(id);
        }

        public List<Bank> GetList()
        {
            return _bankDal.GetListAll();
        }

        public void TAdd(Bank t)
        {
            _bankDal.Insert(t);
        }

        public void TDelete(Bank t)
        {
           _bankDal.Delete(t);
        }

        public Bank TGetById(int id)
        {
            return _bankDal.GetById(id);
        }

        public void TUpdate(Bank t)
        {
            _bankDal.Update(t);
        }
    }
}

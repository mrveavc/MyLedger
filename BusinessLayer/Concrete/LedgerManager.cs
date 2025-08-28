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
    public class LedgerManager : ILedgerService
    {
        ILedgerDal _ledgerDal;

        public LedgerManager(ILedgerDal ledgerDal)
        {
            _ledgerDal = ledgerDal;
        }

        public List<Ledger> GetList()
        {
            return _ledgerDal.GetListAll();
        }

        public List<Ledger> GetListWithLedgerName(int id)
        {
            return _ledgerDal.GetListWithLedger(id);
        }

        public void TAdd(Ledger t)
        {
           _ledgerDal.Insert(t);
        }

        public void TDelete(Ledger t)
        {
            _ledgerDal.Delete(t);
        }

        public Ledger TGetById(int id)
        {
            return _ledgerDal.GetById(id);
        }

        public void TUpdate(Ledger t)
        {
            _ledgerDal.Update(t);
        }
    }
}

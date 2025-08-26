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
            throw new NotImplementedException();
        }

        public void TDelete(Ledger t)
        {
            throw new NotImplementedException();
        }

        public Ledger TGetById(int id)
        {
            return _ledgerDal.GetById(id);
        }

        public void TUpdate(Ledger t)
        {
            throw new NotImplementedException();
        }
    }
}

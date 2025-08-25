using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class LedgerManager : ILedgerService
    {
        public List<Ledger> GetList()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void TUpdate(Ledger t)
        {
            throw new NotImplementedException();
        }
    }
}

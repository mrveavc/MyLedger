using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface ILedgerDal :IGenericDal<Ledger>
    {
        List<Ledger> GetListWithLedger(int id);
        List<Ledger> GetListWithUser();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface ITransactionDal :IGenericDal<Transaction>
    {
        List<Transaction> GetListWithBank(int id);
        List<Transaction> GetListWithLedgerBank(int id);
    }
}

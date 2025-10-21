using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface ITransactionService:IGenericService<Transaction>
    {
        List<Transaction> GetTransactionListWithBank(int id);
        List<Transaction> GetTransactionListWithLedgerBank(int id);
        List<Transaction> GetTransactionListWithUser();
    }
}

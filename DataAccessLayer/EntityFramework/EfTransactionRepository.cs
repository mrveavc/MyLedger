using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class EfTransactionRepository :GenericRepository<Transaction>, ITransactionDal
    {
        public List<Transaction> GetListWithBank(int id)
        {
            using (var c = new Context())
            {
                return c.Transactions.Include(b => b.Bank).Where(x => x.CreatedBy == id).ToList();
            }

        }
		public List<Transaction> GetListWithLedgerBank(int id)
        {
            using (var c = new Context())
            {
                return c.Transactions.Include(b => b.Bank).Include(c=>c.Ledger).Where(x => x.CreatedBy == id).ToList();
            }


        }

    }
}

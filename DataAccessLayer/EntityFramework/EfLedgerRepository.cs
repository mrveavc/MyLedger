using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityFramework
{
    public class EfLedgerRepository :GenericRepository<Ledger>, ILedgerDal 
    {
        public List<Ledger> GetListWithLedger(int id)
        {
            using (var c = new Context())
            {
                var ledgerIds = c.Ledgers
                        .Where(m => m.UserId == id)
                        .Select(m => m.Id)
                        .ToList();

                // Bu LedgerId'lere karşılık gelen Ledger kayıtlarını getir
                var ledgers = c.Ledgers
                               .Where(l => ledgerIds.Contains(l.Id))
                               .ToList();

                return ledgers;
            }
        }
        public List<Ledger> GetListWithUser()
        {
            using (var c = new Context())
            {
                return  c.Ledgers.Include(x => x.User).ToList();

            }
        }
    }
}

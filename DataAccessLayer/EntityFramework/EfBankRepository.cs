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
    public class EfBankRepository :GenericRepository<Bank>, IBankDal
    {
        //public List<Bank> GetListWithLedger(int id)
        //{
        //    using (var c=new Context())
        //    {
        //        var ledgerIds = c.Ledgers
        //         .Where(x => x.UserId == id)
        //         .Select(y => y.Id)
        //         .ToList();

        //        var banks = c.Banks
        //                     .Include(x => x.Ledger)
        //                     .Where(y => ledgerIds.Contains(y.LedgerId))
        //                     .ToList();

        //        return banks;
        //    }
        //}
    }
}

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
    public class EfLedgerMemberRepository:GenericRepository<LedgerMember>, ILedgerMemberDal
    {
        public List<LedgerMember> GetListWithLedgerUser(int id)
        {
            using (var c = new Context())
            {
                return c.LedgerMembers.Include(x => x.User).Include(x => x.Ledger).Where(x => x.Ledger.UserId == id).ToList();
            }
        }
    }
}

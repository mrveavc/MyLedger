using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Concrete;

namespace DataAccessLayer.Abstract
{
    public interface ILedgerMemberDal :IGenericDal<LedgerMember>
    {
        List<LedgerMember> GetListWithLedgerUser(int id);
    }
}

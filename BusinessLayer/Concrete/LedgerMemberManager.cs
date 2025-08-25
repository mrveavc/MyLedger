using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class LedgerMemberManager : ILedgerMemberService
    {
        public List<LedgerMember> GetList()
        {
            throw new NotImplementedException();
        }

        public void TAdd(LedgerMember t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(LedgerMember t)
        {
            throw new NotImplementedException();
        }

        public LedgerMember TGetById(int id)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(LedgerMember t)
        {
            throw new NotImplementedException();
        }
    }
}

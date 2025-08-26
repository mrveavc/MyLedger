using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class LedgerMemberManager : ILedgerMemberService
    {
        ILedgerMemberDal _LedgerMemberDal;

        public LedgerMemberManager(ILedgerMemberDal ledgerMemberDal)
        {
            _LedgerMemberDal = ledgerMemberDal;
        }

        public List<LedgerMember> GetList()
        {
            return _LedgerMemberDal.GetListAll();
        }

        public List<LedgerMember> GetLedgerMemberListWithLedgerUser(int id)
        {
           return _LedgerMemberDal.GetListWithLedgerUser(id);
        }

        public void TAdd(LedgerMember t)
        {
            _LedgerMemberDal.Insert(t);
        }

        public void TDelete(LedgerMember t)
        {
            throw new NotImplementedException();
        }

        public LedgerMember TGetById(int id)
        {
            return _LedgerMemberDal.GetById(id);
        }

        public void TUpdate(LedgerMember t)
        {
            throw new NotImplementedException();
        }
    }
}

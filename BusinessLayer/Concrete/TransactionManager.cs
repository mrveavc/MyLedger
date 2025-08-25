using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class TransactionManager : ITransactionService
    {
        ITransactionDal _transactionDal;

        public TransactionManager(ITransactionDal transactionDal)
        {
            _transactionDal = transactionDal;
        }

        public List<Transaction> GetList()
        {
            throw new NotImplementedException();
        }

        public void TAdd(Transaction t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(Transaction t)
        {
            throw new NotImplementedException();
        }

        public Transaction TGetById(int id)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Transaction t)
        {
            throw new NotImplementedException();
        }
        public List<Transaction> GetTransactionListWithBank(int id)
        {
            return _transactionDal.GetListWithBank(id);
        }

    }
}

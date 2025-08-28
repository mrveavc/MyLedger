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
            return _transactionDal.GetListAll();
        }

        public void TAdd(Transaction t)
        {
            _transactionDal.Insert(t);
        }

        public void TDelete(Transaction t)
        {
            _transactionDal.Delete(t);
        }

        public Transaction TGetById(int id)
        {
            return _transactionDal.GetById(id);
        }

        public void TUpdate(Transaction t)
        {
            _transactionDal.Update(t);
        }
        public List<Transaction> GetTransactionListWithBank(int id)
        {
            return _transactionDal.GetListWithBank(id);
        }

		public List<Transaction> GetTransactionListWithLedgerBank(int id)
		{
            return _transactionDal.GetListWithLedgerBank(id);
		}
	}
}

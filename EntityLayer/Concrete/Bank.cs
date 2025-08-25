using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Bank
    {
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;

        // Navigation
        public Ledger Ledger { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Transaction
    {
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public int? BankId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // Income / Expense /gelir-gider
        public string Category { get; set; }
        public string? Description { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Ledger Ledger { get; set; }
        public Bank Bank { get; set; }
        public AppUser User { get; set; }
    }
}

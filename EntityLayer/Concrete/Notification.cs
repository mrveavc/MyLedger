using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Notification
    {
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public int UserId { get; set; }
        public int? TransactionId { get; set; }

        public string Type { get; set; } // Reminder, Overdue // Hatırlatma, Gecikmiş
        public bool IsSent { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Ledger Ledger { get; set; }
        public AppUser User { get; set; }
        public Transaction Transaction { get; set; }
    }
}

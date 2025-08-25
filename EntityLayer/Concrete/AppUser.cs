using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EntityLayer.Concrete
{
    public class AppUser :IdentityUser<int>
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        public ICollection<Ledger> OwnedLedgers { get; set; }
        public ICollection<LedgerMember> LedgerMembers { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}

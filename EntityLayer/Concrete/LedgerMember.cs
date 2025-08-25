using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class LedgerMember
    {
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public int UserId { get; set; }

        public string Role { get; set; } = "Viewer"; 
        public string? Permissions { get; set; } 

        // Navigation
        public Ledger Ledger { get; set; }
        public AppUser User { get; set; }
    }
}

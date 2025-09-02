﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Ledger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

     
        public AppUser User { get; set; }
        public ICollection<LedgerMember> LedgerMembers { get; set; }
        //public ICollection<Bank> Banks { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}

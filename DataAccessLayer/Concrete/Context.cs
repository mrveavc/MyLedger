using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Concrete;


namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=DESKTOP-KMBI23L\\SQLEXPRESS;Database=MyLedgerDb;TrustServerCertificate=true;Integrated Security=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser -> Ledger (OwnedLedgers)
            modelBuilder.Entity<Ledger>()
                .HasOne(l => l.User)
                .WithMany(u => u.OwnedLedgers)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse bile Ledger kalsın

            // LedgerMember (many-to-one Ledger)
            modelBuilder.Entity<LedgerMember>()
                .HasOne(lm => lm.Ledger)
                .WithMany(l => l.LedgerMembers)
                .HasForeignKey(lm => lm.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            // LedgerMember (many-to-one User)
            modelBuilder.Entity<LedgerMember>()
                .HasOne(lm => lm.User)
                .WithMany(u => u.LedgerMembers)
                .HasForeignKey(lm => lm.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Bank>()
               .Property(b => b.Balance)
                .HasPrecision(18, 2);

            // Transaction -> Ledger
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Ledger)
                .WithMany(l => l.Transactions)
                .HasForeignKey(t => t.LedgerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Transaction>()
                 .Property(t => t.Amount)
                 .HasPrecision(18, 2);

            // Transaction -> Bank (nullable)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Bank)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BankId)
                .OnDelete(DeleteBehavior.SetNull);

            // Transaction -> Creator (ApplicationUser)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification -> Ledger
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Ledger)
                .WithMany()
                .HasForeignKey(n => n.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification -> User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification -> Transaction (nullable)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Transaction)
                .WithMany()
                .HasForeignKey(n => n.TransactionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerMember> LedgerMembers { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
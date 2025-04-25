using Microsoft.EntityFrameworkCore;
using System.IO;

namespace inventory_manager.Models
{
    public class FinanceContext : DbContext
    {
        public DbSet<FinanceAccount> FinanceAccounts { get; set; }
        public DbSet<FinanceTransaction> FinanceTransactions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        string dbFileName = "transaction_record.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbFilePath = GetDatabasePath();

            // if you just provide the name of the file, it will be created in the app's local folder
            // if you provide a path to a file, it will be created in that location
            optionsBuilder.UseSqlite($"Data Source={dbFilePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define any model relationships here
            // For example, a one-to-many relationship between InventoryItem and InventoryCategory
            // you do not need to define EFCore relationships in both the OnModelCreating method and the class itself.
            // you can use data annotations in the class itself as an alternative to the Fluent API.
            // pick one method and stick with it for consistency.
            modelBuilder.Entity<FinanceAccount>(financeAccount =>
            {
                financeAccount.HasKey(account => account.Id);
                financeAccount.Property(account => account.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                financeAccount.Property(account => account.Balance)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<FinanceTransaction>(financeTransaction =>
            {
                financeTransaction.HasKey(transaction => transaction.Id);
                financeTransaction.Property(transaction => transaction.Amount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                financeTransaction.Property(transaction => transaction.Date)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                financeTransaction.Property(transaction => transaction.Description)
                    .HasMaxLength(200);

                financeTransaction.HasOne(transaction => transaction.DebitAccount)
                    .WithMany(account => account.DebitTransactions)
                    .HasForeignKey(transaction => transaction.DebitAccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                financeTransaction.HasOne(transaction => transaction.CreditAccount)
                    .WithMany(account => account.CreditTransactions)
                    .HasForeignKey(transaction => transaction.CreditAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public void EnsureDatabaseCreated()
        {
            // this handles the creation of the database if it doesn't exist
            // it will also apply any pending migrations to bring the database schema up to date
            this.Database.Migrate();
        }

        private string GetDatabasePath()
        {
            // This is the local folder for the current user
            //string localFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WCS_inventory_manager");

            // This is the current directory where the code is executing from
            string localFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }

            string dbFilePath = Path.Combine(localFolderPath, dbFileName);

            return dbFilePath;
        }
    }
}
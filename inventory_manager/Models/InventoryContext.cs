using Microsoft.EntityFrameworkCore;
using System.IO;

namespace inventory_manager.Models
{
    public class InventoryContext : DbContext
    {
        internal DbSet<InventoryItem> InventoryItems { get; set; }
        string dbFileName = "inventory_record.db";

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

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValue("Default Name"); // Default value for Name

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasDefaultValue("Default Description"); // Default value for Description

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasDefaultValue(0); // Default value for Quantity

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0.0m); // Default value for Price

                entity.Property(e => e.DateAdded)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Default value for DateAdded
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

            //if (!File.Exists(dbFilePath))
            //{
            //    File.Create(dbFilePath).Dispose();
            //}

            return dbFilePath;
        }
    }
}

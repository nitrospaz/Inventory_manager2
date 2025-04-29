using Microsoft.EntityFrameworkCore;
using System.IO;

namespace inventory_manager.Models
{
    public class InventoryContext : DbContext
    {
        internal DbSet<InventoryItem> InventoryItems { get; set; }
        internal DbSet<AuditLog> AuditLogs { get; set; }
        internal DbSet<DeliverableItem> DeliverableItems { get; set; }
        internal DbSet<Person> People { get; set; }
        internal DbSet<Comment> Comments { get; set; }

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

            // Configure InventoryItem
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

            // Configure DeliverableItem
            modelBuilder.Entity<DeliverableItem>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(d => d.Description)
                    .HasMaxLength(1000);

                entity.Property(d => d.Project)
                    .HasMaxLength(200);

                // Relationships
                entity.HasMany(d => d.Comments)
                      .WithOne()
                      .HasForeignKey(c => c.DeliverableItemId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(d => d.AssignedTo)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("DeliverableItemAssignedTo"));

                entity.HasMany(d => d.NotifyList)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("DeliverableItemNotifyList"));
            });

            // Configure Person
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.Id); // Set Id as the primary key
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Email).HasMaxLength(200);
            });

            // Configure Comment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => new { c.DeliverableItemId, c.Timestamp }); // Composite Key
                entity.Property(c => c.Text).IsRequired().HasMaxLength(500);

                // Configure the Author navigation property
                entity.HasOne(c => c.Author)
                      .WithMany()
                      .HasForeignKey(c => c.AuthorId) // Use AuthorId as the foreign key
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
            });

            // Configure AuditLog
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id); // Set Id as the primary key

                entity.Property(a => a.Timestamp)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Default value for Timestamp

                entity.Property(a => a.Action)
                      .IsRequired()
                      .HasMaxLength(200); // Limit the length of the Action field

                entity.Property(a => a.Details)
                      .HasMaxLength(1000); // Limit the length of the Details field
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

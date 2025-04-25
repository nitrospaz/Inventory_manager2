using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory_manager.Models
{
    internal class InventoryItem
    {
        // you do not need to define EFCore relationships in both the OnModelCreating method and the class itself.
        // you can use data annotations in the class itself as an alternative to the Fluent API.
        // pick one method and stick with it for consistency.

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }

        public InventoryItem(int id, string name, string description, int quantity, decimal price, DateTime dateAdded)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            DateAdded = dateAdded;
        }

        public override string ToString()
        {
            return $"{Name} (ID: {Id}) - {Description}, Quantity: {Quantity}, Price: {Price:C}, Added on: {DateAdded:d}";
        }
    }
}

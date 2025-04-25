using inventory_manager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace inventory_manager.Services
{
    class InventoryDataService
    {
        public InventoryDataService()
        {
            using (var db = new InventoryContext())
            {
                db.EnsureDatabaseCreated();
            }
        }

        public async Task AddInventoryItemAsync(InventoryItem item)
        {
            try
            {
                using (var db = new InventoryContext())
                {
                    await db.InventoryItems.AddAsync(item);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding inventory item: {ex.Message}");
            }
        }
        public async Task UpdateInventoryItemAsync(InventoryItem item)
        {
            try
            {
                using (var db = new InventoryContext())
                {
                    var existingItem = await db.InventoryItems.FindAsync(item.Id);
                    if (existingItem != null)
                    {
                        existingItem.Name = item.Name;
                        existingItem.Description = item.Description;
                        existingItem.Quantity = item.Quantity;
                        existingItem.Price = item.Price;
                        existingItem.DateAdded = item.DateAdded;

                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating inventory item: {ex.Message}");
            }
        }

        public List<InventoryItem> GetInventoryItems()
        {
            try
            {
                using (var db = new InventoryContext())
                {
                    return db.InventoryItems.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting inventory items: {ex.Message}");
                return new List<InventoryItem>();
            }
        }

        public async Task DeleteInventoryItemAsync(int id)
        {
            try
            {
                using (var db = new InventoryContext())
                {
                    var item = await db.InventoryItems.FindAsync(id);
                    if (item != null)
                    {
                        db.InventoryItems.Remove(item);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting inventory item: {ex.Message}");
            }
        }
    }
}

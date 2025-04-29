using inventory_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inventory_manager.Services
{
    class ProjectDataService
    {
        private readonly InventoryContext _context;

        public ProjectDataService(InventoryContext context)
        {
            _context = context;
        }

        // CRUD Operations for Person
        public async Task<List<Person>> GetAllPeopleAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
            return await _context.People.FindAsync(id);
        }

        public async Task AddPersonAsync(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            await LogActionAsync("AddPerson", $"Added person: {person.Name}");
        }

        public async Task UpdatePersonAsync(Person person)
        {
            _context.People.Update(person);
            await _context.SaveChangesAsync();
            await LogActionAsync("UpdatePerson", $"Updated person: {person.Name}");
        }

        public async Task DeletePersonAsync(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
                await LogActionAsync("DeletePerson", $"Deleted person: {person.Name}");
            }
        }

        // CRUD Operations for DeliverableItem
        public async Task<List<DeliverableItem>> GetAllDeliverableItemsAsync()
        {
            return await _context.DeliverableItems
                .Include(d => d.Comments)
                .Include(d => d.AssignedTo)
                .Include(d => d.NotifyList)
                .ToListAsync();
        }

        public async Task<DeliverableItem> GetDeliverableItemByIdAsync(int id)
        {
            return await _context.DeliverableItems
                .Include(d => d.Comments)
                .Include(d => d.AssignedTo)
                .Include(d => d.NotifyList)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDeliverableItemAsync(DeliverableItem item)
        {
            _context.DeliverableItems.Add(item);
            await _context.SaveChangesAsync();
            await LogActionAsync("AddDeliverableItem", $"Added deliverable item: {item.Name}");
        }

        public async Task UpdateDeliverableItemAsync(DeliverableItem item)
        {
            _context.DeliverableItems.Update(item);
            await _context.SaveChangesAsync();
            await LogActionAsync("UpdateDeliverableItem", $"Updated deliverable item: {item.Name}");
        }

        public async Task DeleteDeliverableItemAsync(int id)
        {
            var item = await _context.DeliverableItems.FindAsync(id);
            if (item != null)
            {
                _context.DeliverableItems.Remove(item);
                await _context.SaveChangesAsync();
                await LogActionAsync("DeleteDeliverableItem", $"Deleted deliverable item: {item.Name}");
            }
        }

        // CRUD Operations for Comment
        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            await LogActionAsync("AddComment", $"Added comment: {comment.Text}");
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            await LogActionAsync("DeleteComment", $"Deleted comment: {comment.Text}");
        }

        // Audit Logging
        private async Task LogActionAsync(string action, string details)
        {
            var log = new AuditLog
            {
                Timestamp = DateTime.UtcNow,
                Action = action,
                Details = details
            };
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;

namespace inventory_manager.Models
{
    // this is a class that represents a deliverable item in a project management context.
    // it is very customizable and can be used to represent various types of items.

    // Example:

    //var person1 = new Person("John Doe", "Developer", "john.doe@example.com");
    //var person2 = new Person("Jane Smith", "Manager", "jane.smith@example.com");

    //var item = new DeliverableItem(
    //    id: 1,
    //    name: "Design Document",
    //    description: "Create the initial design document",
    //    status: ItemStatus.InProgress,
    //    type: ItemType.Task,
    //    priority: PriorityLevel.High,
    //    categories: new HashSet<string> { "Documentation", "Design" },
    //    project: "Project A",
    //    assignedTo: new HashSet<Person> { person1 },
    //    notifyList: new HashSet<Person> { person2 },
    //    comments: new List<Comment>
    //    {
    //    new Comment("Initial draft completed.", person1, DateTime.Now.AddDays(-1))
    //    },
    //    dueDate: DateTime.Now.AddDays(7),
    //    dateAdded: DateTime.Now
    //);
    
    //// Add a new tag
    //deliverable.AddTag("HighPriority");

    //// Remove a tag
    //deliverable.RemoveTag("Phase1");

    //// Check if a tag exists
    //bool hasTag = deliverable.HasTag("Urgent");

    //// Add a person to the NotifyList
    //item.AddToNotifyList(new Person("Team Alpha", "Development Team", "team.alpha@example.com"));

    //// Remove a person from the NotifyList
    //item.RemoveFromNotifyList(person2);

    //// Add a new category
    //bool addedCategory = item.AddCategory("Phase 1"); // Returns true
    //bool duplicateCategory = item.AddCategory("Design"); // Returns false (already exists)

    //// Remove a category
    //bool removedCategory = item.RemoveCategory("Documentation"); // Returns true
    //bool nonExistentCategory = item.RemoveCategory("NonExistent"); // Returns false

    //// Add a new comment
    //item.AddComment(new Comment("Final version submitted.", "John Doe", DateTime.Now.AddDays(1)));

    //// Add a new person
    //item.AddAssignedTo(new Person("Team Alpha", "Development Team"));

    //// Add a new assigned individual
    //bool addedPerson = item.AddAssignedTo("Team Alpha"); // Returns true
    //bool duplicatePerson = item.AddAssignedTo("John Doe"); // Returns false (already exists)

    //// Remove an assigned individual
    //bool removedPerson = item.RemoveAssignedTo("Jane Smith"); // Returns true
    //bool nonExistentPerson = item.RemoveAssignedTo("NonExistent"); // Returns false

    class DeliverableItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Project { get; set; } // The project to which the item belongs
        public ItemStatus Status { get; set; } // enum
        public ItemType Type { get; set; } // enum
        public PriorityLevel Priority { get; set; } // enum
        // HashSet is just a list that ensures unique entries
        public HashSet<string> Categories { get; set; } // e.g., "Electronics", "Furniture", "Software", ect.
        public HashSet<string> Tags { get; set; }
        public HashSet<Person> AssignedTo { get; set; }  // The people or team responsible for the item
        public HashSet<Person> NotifyList { get; set; } // The people or team to be notified about the item
        public List<Comment> Comments { get; set; } // Additional notes or comments about the item
        public DateTime DateAdded { get; set; } // The date the item was added
        public DateTime DueDate { get; set; } // The date by which the item should be delivered

        // Parameterless constructor for EF Core
        public DeliverableItem()
        {
            Categories = new HashSet<string>();
            Tags = new HashSet<string>();
            AssignedTo = new HashSet<Person>();
            NotifyList = new HashSet<Person>();
            Comments = new List<Comment>();
        }

        // Constructor for application use
        public DeliverableItem(int id, string name, string description, ItemStatus status, ItemType type, PriorityLevel priority, IEnumerable<string> categories, string project, IEnumerable<Person> assignedTo, IEnumerable<Person> notifyList, IEnumerable<Comment> comments, IEnumerable<string> tags, DateTime dueDate, DateTime dateAdded)
            : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            Type = type;
            Priority = priority;
            Project = project;
            DueDate = dueDate;
            DateAdded = dateAdded;

            if (categories != null) Categories = new HashSet<string>(categories);
            if (tags != null) Tags = new HashSet<string>(tags);
            if (assignedTo != null) AssignedTo = new HashSet<Person>(assignedTo);
            if (notifyList != null) NotifyList = new HashSet<Person>(notifyList);
            if (comments != null) Comments = new List<Comment>(comments);
        }

        public override string ToString()
        {
            string categories = string.Join(", ", Categories);
            string assignedTo = string.Join(", ", AssignedTo);
            string notifyList = string.Join(", ", NotifyList);
            string comments = string.Join("\n", Comments);
            string tags = string.Join(", ", Tags);

            return $"{Name} (ID: {Id}) - {Description}, Status: {Status}, Type: {Type}, Priority: {Priority}, Categories: [{categories}], Tags: [{tags}], Assigned To: [{assignedTo}], Notify List: [{notifyList}], Comments:\n{comments}, Due Date: {DueDate:d}, Added on: {DateAdded:d}";
        }

        // Add a tag
        public bool AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Tag cannot be null or empty.");
            return Tags.Add(tag); // Returns true if added, false if it already exists
        }

        // Remove a tag
        public bool RemoveTag(string tag)
        {
            return Tags.Remove(tag); // Returns true if removed, false if it doesn't exist
        }

        // Check if a tag exists
        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        // add a person to the NotifyList
        public bool AddToNotifyList(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            return NotifyList.Add(person); // Returns true if added, false if it already exists
        }

        // remove a person from the NotifyList
        public bool RemoveFromNotifyList(Person person)
        {
            return NotifyList.Remove(person); // Returns true if removed, false if it doesn't exist
        }

        // add a category
        public bool AddCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Category cannot be null or empty.");
            }
            return Categories.Add(category); // Returns true if added, false if it already exists
        }

        // remove a category
        public bool RemoveCategory(string category)
        {
            return Categories.Remove(category); // Returns true if removed, false if it doesn't exist
        }

        // Add a person to the assigned list
        public bool AddAssignedTo(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            return AssignedTo.Add(person);
        }

        // Remove a person from the assigned list
        public bool RemoveAssignedTo(Person person)
        {
            return AssignedTo.Remove(person);
        }

        // Add a comment
        public void AddComment(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));
            if (comment.DeliverableItemId != Id)
                throw new InvalidOperationException("The comment's DeliverableItemId does not match this DeliverableItem's Id.");

            Comments.Add(comment);
        }

        // Remove a comment
        public bool RemoveComment(Comment comment)
        {
            return Comments.Remove(comment);
        }
    }
}

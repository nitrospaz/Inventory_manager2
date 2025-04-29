using System;

namespace inventory_manager.Models
{
    // Example:
    //var author = new Person("John Doe", "Designer");
    //var comment = new Comment("Initial draft completed.", author, DateTime.Now);

    //Console.WriteLine(comment.ToString());
    //// Output: John Doe (Designer) (4/28/2025 10:00 AM): Initial draft completed.
    ///
    public class Comment
    {
        public string Text { get; set; } // The content of the comment
        public Person Author { get; set; } // Navigation property // The person who made the comment
        public int AuthorId { get; set; } // Foreign key referencing Person.Id
        public DateTime Timestamp { get; set; } // When the comment was made
        public int DeliverableItemId { get; set; } // The ID of the associated DeliverableItem

        // Parameterless constructor for EF Core
        public Comment() { }

        // Constructor for application use
        public Comment(string text, Person author, DateTime timestamp, int deliverableItemId)
        {
            Text = text;
            Author = author;
            AuthorId = author?.Id ?? 0; // Set the foreign key
            Timestamp = timestamp;
            DeliverableItemId = deliverableItemId;
        }

        public override string ToString()
        {
            return $"{Author} ({Timestamp:g}): {Text} (Deliverable ID: {DeliverableItemId})";
        }
    }
}

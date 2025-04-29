using System;

namespace inventory_manager.Models
{
    /// <summary>
    /// Represents an audit log entry.
    /// </summary>
    
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
    }
}

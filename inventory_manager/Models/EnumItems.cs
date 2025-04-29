using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory_manager.Models
{
    public enum ItemStatus
    {
        Pending = 0,       // Explicitly assigned value 0
        InProgress = 1,    // Explicitly assigned value 1
        Completed = 2      // Explicitly assigned value 2
    }

    public enum ItemType
    {
        Deliverable = 0,   // Explicitly assigned value 0
        Task = 1,          // Explicitly assigned value 1
        Milestone = 2,     // Explicitly assigned value 2
        Objective = 3,     // Explicitly assigned value 3
        ActionItem = 4,    // Explicitly assigned value 4
        Step = 5,          // Explicitly assigned value 5
        Phase = 6,         // Explicitly assigned value 6
        Component = 7,     // Explicitly assigned value 7
        Job = 8            // Explicitly assigned value 8
    }

    public enum PriorityLevel
    {
        Low = 1,           // Explicitly assigned value 1
        Medium = 2,        // Explicitly assigned value 2
        High = 3           // Explicitly assigned value 3
    }
    public enum PersonRole
    {
        SoftwareDeveloper = 0,
        Manager = 1,
        Designer = 2,
        Tester = 3,
        TeamLead = 4,
        Engineer = 5,
        Architect = 6,
        Administrator = 7,
        ProjectManager = 8,
        Stakeholder = 9,
        Subcontractor = 10,
        Vendor = 11,
        Supplier = 12,
        Team = 13,
        Other = 99 // For roles not explicitly defined
    }
}

using System;
using System.Text.RegularExpressions;

namespace inventory_manager.Models
{
    public class Person
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } // The name of the person or team
        public PersonRole Role { get; set; }// The role of the person (optional)
        public string Email { get; set; } // The email address of the person

        public Person(string name, PersonRole role = PersonRole.Other, string email = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");

            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                throw new ArgumentException("Invalid email format.");

            Name = name;
            Role = role;
            Email = email;
        }

        public override string ToString()
        {
            string rolePart = Role != PersonRole.Other ? $" ({Role})" : "";
            string emailPart = string.IsNullOrWhiteSpace(Email) ? "" : $" - {Email}";
            return $"{Name}{rolePart}{emailPart}";
        }

        private bool IsValidEmail(string email)
        {
            // Simple regex for email validation
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}

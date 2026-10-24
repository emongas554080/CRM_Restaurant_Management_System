using System;

namespace CRM.domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string CustomerType { get; set; } = string.Empty;

        public string FavoriteFood { get; set; } = string.Empty;

        public string VisitFrequency { get; set; } = string.Empty;

        public string Feedback { get; set; } = string.Empty;

        public DateTime DateCollected { get; set; } = DateTime.Now;

        public bool IsArchived { get; set; } = false;

        public DateTime? DateArchived { get; set; }
    }
}
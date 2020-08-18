using System;
using System.Collections.Generic;

namespace BangazonAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<PaymentType> Payments { get; set; } = new List<PaymentType>();
    }
}

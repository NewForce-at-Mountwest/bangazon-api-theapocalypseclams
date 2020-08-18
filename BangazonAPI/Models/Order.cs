using System.Collections.Generic;

namespace BangazonAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}

﻿namespace BangazonAPI.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        public string AcctNumber { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }
}

using System;

namespace BangazonAPI.Models
{
    public class ComputerEmployee
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ComputerId { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime UnassignDate { get; set; }

    }
}

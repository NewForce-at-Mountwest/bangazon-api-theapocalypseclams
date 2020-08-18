using System.Collections.Generic;

namespace BangazonAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Budget { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }

}

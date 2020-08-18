namespace BangazonAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public bool isSupervisor { get; set; }
        //TODO: From Ticket #6:
        //The employee's department name should be included in the employee representation
        //A representation of the computer that the employee is currently using should be included in the employee representation
        //You'll probably want to add properties of type Department and Computer to this model eventually, but it doesn't have to be right now-- you can discuss as a team whether you want to add them now or later
    }
}

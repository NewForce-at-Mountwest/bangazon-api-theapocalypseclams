using System;

namespace BangazonAPI.Models
{
    public class TrainingProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxAttendees { get; set; }
        // TODO:       From Ticket #9:
        //Employees who signed up for a training program should be included in the response
        //We could go ahead and add a list of Employees here or wait and do it later, up to y'all
    }
}

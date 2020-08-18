namespace BangazonAPI.Models
{
    public class EmployeeTraining
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int TrainingProgramId { get; set; }
        //TODO:You don't always necessarily need models for join tables-- it really depends on the situation. It's fine to merge them in now but I just wanted to put it on your radar so that you can go back and delete any you don't use at the end of the sprint
    }
}

namespace scs_web_game.Models
{
    public enum Role
    {
        Employee = 0,
        ProjectManager = 1,
        DepartmentsHead = 2
    }
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public  string  ImgFileName { get; set; } = null!;
        public Role Role { get; set; }
    }
}

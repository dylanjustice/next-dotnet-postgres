namespace DylanJustice.Demo.Presentation.Web.Models.Dtos.Employees
{
    public class EmployeeDto : EntityDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string IpAddress { get; set; }
    }
}
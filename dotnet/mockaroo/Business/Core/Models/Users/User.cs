using AndcultureCode.CSharp.Core.Models.Entities;

namespace Mockaroo.Business.Core.Models.Users
{
    public class User : Entity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? IpAddress { get; set; }
    }
}
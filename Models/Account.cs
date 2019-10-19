using System;
namespace ConnectOnCommuteBackend.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Question { get; set; }
        public string Role { get; set; }
        public DateTime SeniorityDate { get; set; }
        public bool FindableStatus { get; set; }
    }
}

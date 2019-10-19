using System;
namespace ConnectOnCommuteBackend.Models
{
    public class SignupRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string Question { get; set; }
    }
}

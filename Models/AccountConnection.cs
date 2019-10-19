using System;
namespace ConnectOnCommuteBackend.Models
{
    public class AccountConnection
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int TargetId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

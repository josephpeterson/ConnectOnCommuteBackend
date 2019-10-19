using System;
namespace ConnectOnCommuteBackend.Models
{
    public class AccountNotification
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public bool Dismissed { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

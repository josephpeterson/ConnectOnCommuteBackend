using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectOnCommuteBackend.Models
{
    public class UserPosition
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}

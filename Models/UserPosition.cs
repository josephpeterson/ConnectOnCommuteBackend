using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectOnCommuteBackend.Models
{
    public class UserPosition
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}

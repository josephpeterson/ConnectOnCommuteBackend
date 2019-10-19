using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectOnCommuteBackend.Models
{
    public class UserPosition
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        [Column("Longitude")]
        public string LongitudeStr { get; set; }
        [Column("Latitude")]
        public string LatitudeStr { get; set; }
        public double Longitude => Convert.ToDouble(LongitudeStr);
        public double Latitude => Convert.ToDouble(LatitudeStr);
        public DateTime Timestamp { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}

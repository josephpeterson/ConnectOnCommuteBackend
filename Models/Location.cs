using System;
using System.Device.Location;

namespace ConnectOnCommuteBackend.Models
{
    public class UserCoords
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }


        public double DistanceTo(double lat,double longitude)
        {
            var coord1 = new GeoCoordinate(lat, longitude);
            var coord2 = new GeoCoordinate(Latitude, Longitude);
            return coord1.GetDistanceTo(coord2);
        }
    }
}

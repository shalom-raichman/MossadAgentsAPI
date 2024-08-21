using System.ComponentModel.DataAnnotations;

namespace MossadAgentsAPI.Models
{
    public class MapLocation
    {
        public MapLocation(double x, double y)
        {
            X = x;
            Y = y;
        }
        public int Id { get; set; }
        [Range(0, 1000)]
        public double X { get; set; }
        [Range(0, 1000)]
        public double Y { get; set; }
    }
}

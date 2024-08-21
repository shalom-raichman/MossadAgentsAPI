using System.ComponentModel.DataAnnotations;

namespace MossadAgentsAPI.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Range(0, 1000)]
        public double X { get; set; }
        [Range(0, 1000)]
        public double Y { get; set; }
    }
}

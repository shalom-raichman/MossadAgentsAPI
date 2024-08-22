using System.ComponentModel.DataAnnotations;

namespace MossadAgentsAPI.Models
{
    public class Coordinates
    {
        public Coordinates() { }
        public Coordinates(double x, double y)
        {
            X = x;
            Y = y;
        }
        [Key]
        public int Id { get; set; }
        [Range(0, 1000)]
        public double X { get; set; }
        [Range(0, 1000)]
        public double Y { get; set; }
    }
}

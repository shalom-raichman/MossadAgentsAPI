using MossadAgentsAPI.Enums;

namespace MossadAgentsAPI.Models
{
    public class Target
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public Location Location { get; set; }
        public TargetStatus Status { get; set; }
    }
}

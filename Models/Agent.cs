using MossadAgentsAPI.Enums;

namespace MossadAgentsAPI.Models
{
    public class Agent
    {
        public string Image { get; set; }
        public string Nickname { get; set; }
        public Location Location { get; set; }
        public AgentStatus Status { get; set; }
    }
}

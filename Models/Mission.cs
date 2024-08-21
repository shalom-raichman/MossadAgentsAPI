using System;

namespace MossadAgentsAPI.Models
{
    public class Mission
    {
        public Guid Id { get; set; }
        public Agent Agent { get; set; }
        public Target Target { get; set; }
        public double TimeLeft { get; set; }
        public double ActualExecutionTime { get; set; }

    }
}

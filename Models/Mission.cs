using MossadAgentsAPI.Enums;
using System;

namespace MossadAgentsAPI.Models
{
    public class Mission
    {
        public int? Id { get; set; }
        public Agent Agent { get; set; }
        public Target Target { get; set; }
        public double? ExecutionTime { get; set; }
        public double? ActualExecutionTime { get; set; }
        public MissionStatus? Status { get; set; }
        public Double? distance { get; set; }

    }
}

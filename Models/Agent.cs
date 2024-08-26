using MossadAgentsAPI.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;

namespace MossadAgentsAPI.Models
{
    public class Agent
    {
        [Key]
        public int? id { get; set; }
        public string photoUrl { get; set; }
        public string nickname { get; set; }
        public Coordinates? coordinates { get; set; }
        public AgentStatus? Status { get; set; }
        public int? Kills { get; set; } = 0;
    }
}

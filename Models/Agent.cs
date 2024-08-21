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
        public Guid? Id { get; set; }
        public string Image { get; set; }
        public string Nickname { get; set; }
        public MapLocation? Location { get; set; }
        public AgentStatus? Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Data
{
    public class MossadAgentsAPIContext : DbContext
    {
        public MossadAgentsAPIContext (DbContextOptions<MossadAgentsAPIContext> options)
            : base(options)
        {
            //Seed();
            Database.EnsureCreated();
        }

        //public void Seed()
        //{
        //    Agent agent = new Agent
        //    { 
        //        nickname = "test",
        //        photoUrl = "test"
            
        //    };

        //    Target target = new Target
        //    {
        //        name = "test",
        //        photoUrl = "test"
        //    };
        //    Mission mission = new Mission
        //    {
        //        Agent = agent,
        //        Target = target,
        //        Status = MissionStatus.Proposal
        //    };

        //    Agents.Add(agent);
        //    Targets.Add(target);
        //    Missions.Add(mission);
        //    SaveChanges();
        //}

        public DbSet<MossadAgentsAPI.Models.Agent> Agents { get; set; } = default!;
        public DbSet<MossadAgentsAPI.Models.Target> Targets { get; set; } = default!;
        public DbSet<MossadAgentsAPI.Models.Mission> Missions { get; set; } = default!;
        public DbSet<MossadAgentsAPI.Models.Coordinates> coordinates { get; set; } = default!;

    }
}

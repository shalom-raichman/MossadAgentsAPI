using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Data
{
    public class MossadAgentsAPIContext : DbContext
    {
        public MossadAgentsAPIContext (DbContextOptions<MossadAgentsAPIContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<MossadAgentsAPI.Models.Agent> Agents { get; set; } = default!;
        public DbSet<MossadAgentsAPI.Models.Target> Targets { get; set; } = default!;
        public DbSet<MossadAgentsAPI.Models.Mission> Missions { get; set; } = default!;
        public DbSet<MossadAgentsAPI.Models.Coordinates> coordinates { get; set; } = default!;

    }
}

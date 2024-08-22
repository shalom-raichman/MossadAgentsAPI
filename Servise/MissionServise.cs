using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class MissionServise
    {
        private readonly MossadAgentsAPIContext _context;

        MissionServise(MossadAgentsAPIContext context)
        {
            _context = context;
        }

        public static bool IsInRange(Coordinates targetCoordinates, Coordinates agentCoordinates)
        {
                double distans = Calculations.GetDistans(targetCoordinates, agentCoordinates);
                if (distans <= 200) {return true;}
                else {return false;}
        }

        public static Agent SearchAgentInRange(Target target, DbSet<MossadAgentsAPI.Models.Agent> agents)
        {
            foreach (var agent in agents)
            {
                if (IsInRange(target.coordinates, agent.coordinates))
                {
                    return agent;
                }
            }
            return null;
        }

        // return mission if rools valid
        public static Mission CreteMission(Target target, DbSet<MossadAgentsAPI.Models.Agent> agents)
        {
            Agent agent = SearchAgentInRange(target, agents);
            if (agent != null)
            {
                Mission mission = new Mission();
                mission.Agent = agent;
                mission.Target = target;
                mission.Status = MissionStatus.Proposal;
            }
            return null;
        }

        public static List<Mission> UnsportedMissins(DbSet<MossadAgentsAPI.Models.Mission> missions)
        {
            List<Mission> missions_list = new List<Mission>();
            foreach (var mission in missions)
            {
                if(!IsInRange(mission.Target.coordinates, mission.Agent.coordinates))
                {
                    missions.Add(mission);
                }
            }

            return missions_list;
        }


    }
}

using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class MissionServise
    {
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

        public static Mission CreteMission(Target target, DbSet<MossadAgentsAPI.Models.Agent> agents)
        {
            Agent agent = SearchAgentInRange(target, agents);

            Mission mission = new Mission();
            mission.Agent = agent;
            mission.Target = target;
            mission.Status = MissionStatus.Proposal;

            return mission;
        }

        public static void DeleteUnsportedMissins(DbSet<MossadAgentsAPI.Models.Mission> missions)
        {
            foreach (var mission in missions)
            {
                
            }
        }


    }
}

using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class MissionServise
    {
        private readonly MossadAgentsAPIContext _context;

        public MissionServise() { }
        MissionServise(MossadAgentsAPIContext context)
        {
            _context = context;
        }

        public bool IsInRange(Coordinates targetCoordinates, Coordinates agentCoordinates)
        {
                double distans = Calculations.GetDistans(targetCoordinates, agentCoordinates);
                if (distans <= 200) {return true;}
                else {return false;}
        }

        public Agent SearchAgentInRange(Target target)
        {
            foreach (var agent in _context.Agents)
            {
                if (IsInRange(target.coordinates, agent.coordinates))
                {
                    return agent;
                }
            }
            return null;
        }

        // return mission if rools valid
        public async Task CreteMission(Target target)
        {
            Agent agent = SearchAgentInRange(target);
            if (agent != null && agent.Status != AgentStatus.OnMission)
            {
                Mission mission = new Mission();
                mission.Agent = agent;
                mission.Target = target;
                mission.Status = MissionStatus.Proposal;
                agent.Status = AgentStatus.OnMission;

                _context.Agents.Update(agent);
                await _context.Missions.AddAsync(mission);
                await _context.SaveChangesAsync();
            }
        }

        public void DleteUnsportedMissins()
        {
            // bug to fix !!!!!!!!!

            List<Mission> missions_list = new List<Mission>();
            foreach (var mission in _context.Missions.ToList())
            {
                if(!IsInRange(mission.Target.coordinates, mission.Agent.coordinates))
                {
                    _context.Missions.Remove(mission);
                    _context.SaveChanges();
                    Console.WriteLine($"mission {mission.Id} deleted from data base");
                }
            }
        }


    }
}

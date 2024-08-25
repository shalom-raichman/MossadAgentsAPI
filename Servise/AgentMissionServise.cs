using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class AgentMissionServise
    {
        private readonly MossadAgentsAPIContext _context;
        public AgentMissionServise(MossadAgentsAPIContext context)
        {
            _context = context;
        }

        public bool IsInRange(Coordinates targetCoordinates, Coordinates agentCoordinates)
        {
            if (agentCoordinates == null || targetCoordinates == null) {return false;}
                double distans = Calculations.GetDistans(targetCoordinates, agentCoordinates);
                if (distans <= 200) {return true;}
                else {return false;}
        }

        public Target SearchAgentInRange(Agent agent)
        {
            foreach (var target in _context.Targets.Include(a => a.coordinates))
            {
                if (IsInRange(agent.coordinates, target.coordinates))
                {
                    return target ;
                }
            }
            return null;
        }

        // return mission if rools valid
        public async Task CreteMission(Agent agent)
        {
            Target target = SearchAgentInRange(agent);
            if (target != null && target.Status != TargetStatus.OnPresud)
            {
                Mission mission = new Mission();
                mission.Agent = agent;
                mission.Target = target;
                mission.Status = MissionStatus.Proposal;
                agent.Status = AgentStatus.OnMission;
                target.Status = TargetStatus.OnPresud;

                _context.Targets.Update(target);
                _context.Agents.Update(agent);
                await _context.Missions.AddAsync(mission);
                await _context.SaveChangesAsync();
            }
        }


    }
}

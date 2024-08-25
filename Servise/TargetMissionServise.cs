using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class TargetMissionServise
    {
        private readonly MossadAgentsAPIContext _context;
        public TargetMissionServise(MossadAgentsAPIContext context)
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
            foreach (var agent in _context.Agents.Include(a => a.coordinates))
            {
                if (IsInRange(target.coordinates, agent.coordinates))
                {
                    return agent ;
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
                target.Status = TargetStatus.OnPresud;

                _context.Agents.Update(agent);
                await _context.Missions.AddAsync(mission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DleteUnsportedMissins()
        {
            List<Mission> missions_list = new List<Mission>();
            var missions = await _context.Missions.Include(m => m.Agent).ThenInclude(a => a.coordinates).ToListAsync();
            foreach (var mission in missions)
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

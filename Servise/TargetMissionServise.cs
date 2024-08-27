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

        // create mission if rools valid
        public async Task CreteMission(Target target)
        {
            Agent agent = SearchAgentInRange(target);
            if (agent != null && agent.Status == AgentStatus.Sleep && target.Status == TargetStatus.Alive)
            {
                Mission mission = new Mission();
                mission.Agent = agent;
                mission.Target = target;
                mission.Status = MissionStatus.Proposal;

                await _context.Missions.AddAsync(mission);
                await _context.SaveChangesAsync();

                await DleteUnsportedMissins2(mission);
            }
        }

        // return true if target and agent in range
        public bool IsInRange(Coordinates targetCoordinates, Coordinates agentCoordinates)
        {
                double distans = Calculations.GetDistans(targetCoordinates, agentCoordinates);
                if (distans <= 200) {return true;}
                else {return false;}
        }

        // search if there is angent in the rang of the target
        // return agent if true and null if false
        public Agent SearchAgentInRange(Target target)
        {
            foreach (var agent in _context.Agents.Include(a => a.coordinates))
            {
                if (agent.Status == AgentStatus.OnMission) continue;
                if (IsInRange(target.coordinates, agent.coordinates))
                {
                    return agent ;
                }
            }
            return null;
        }


        public async Task DleteUnsportedMissins()
        {
            List<Mission> missions_list = new List<Mission>();
            var missions = await _context.Missions
                .Include(m => m.Agent)
                .ThenInclude(a => a.coordinates)
                .ToListAsync();
            foreach (var mission in missions)
            {
                if (mission == null || mission.Target == null || mission.Agent == null) continue;
                if(!IsInRange(mission.Target.coordinates, mission.Agent.coordinates))
                {
                    _context.Missions.Remove(mission);
                    _context.SaveChanges();
                    Console.WriteLine($"mission {mission.Id} deleted from data base");
                }
            }
        }
        
        public async Task DleteUnsportedMissins2(Mission mission)
        {
            List<Mission> missions_list = new List<Mission>();
            var missions = await _context.Missions
                .Include(m => m.Agent)
                .ThenInclude(a => a.coordinates)
                .ToListAsync();
            foreach (var item in missions)
            {
                if (item.Agent.id == mission.Agent.id && mission.Id != item.Id)
                {
                    _context.Missions.Remove(item);
                    _context.SaveChanges();
                }
            }
        }


    }
}

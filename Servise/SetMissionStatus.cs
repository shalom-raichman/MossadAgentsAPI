using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class SetMissionStatus
    {
        private readonly MossadAgentsAPIContext _context;
        public SetMissionStatus(MossadAgentsAPIContext context)
        {
            _context = context;
        }

        // Set Sttatus To Assignment To Task
        // return true if rools steel valid
        public async Task<bool> SetSttatusToAssignmentToTask(Mission mission)
        {
            double dist = Calculations.GetDistans(mission.Agent.coordinates, mission.Target.coordinates);


            if (mission.Status == MissionStatus.Proposal)
            {
                if (dist > 200)
                {
                    _context.Missions.Remove(mission);
                    await _context.SaveChangesAsync();
                    return false;
                }

                mission.Agent.Status = AgentStatus.OnMission;
                mission.Target.Status = TargetStatus.OnPresud;
                mission.Status = MissionStatus.AssignmentToTask;
                _context.Agents.Update(mission.Agent);
                _context.Targets.Update(mission.Target);
                _context.Missions.Update(mission);
                _context.SaveChanges();
                await DeleteAgentAtherMissions(mission);
                return true;
            }
            return false;
        }

        // delete other agent proposels 
        public async Task DeleteAgentAtherMissions(Mission mission)
        {
            var missions = _context.Missions.Include(m => m.Agent);

            foreach (var item in missions)
            {
                if (item.Agent.id == mission.Agent.id && mission.Id != item.Id)
                {
                    _context.Missions.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}

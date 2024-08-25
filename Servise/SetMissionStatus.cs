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

        public async Task SetSttatusToAssignmentToTask(Mission mission)
        {
            if (mission.Status == MissionStatus.Proposal)
            {
                mission.Status = MissionStatus.AssignmentToTask;
                _context.Missions.Update(mission);
                _context.SaveChanges();
            }
        }
    }
}

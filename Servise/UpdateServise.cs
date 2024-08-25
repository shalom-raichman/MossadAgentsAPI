using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;
using System.Collections.Generic;

namespace MossadAgentsAPI.Servise
{
    public class UpdateServise
    {
        private readonly MossadAgentsAPIContext _context;
        public UpdateServise(MossadAgentsAPIContext context)
        {
            _context = context;
        }

        public async Task UpdateMission()
        {
            var missions = await _context.Missions
                .Include(m => m.Target)
                .ThenInclude(t => t.coordinates)
                .Include(m => m.Agent)
                .ThenInclude(m => m.coordinates)
                .ToListAsync();

            foreach (var mission in missions)
            {
                if (mission == null) { continue; }

                if (IsMissionsToUpdate(mission)){

                    var agentCoordinates = mission.Agent.coordinates;
                    var targetCoordinates = mission.Target.coordinates;

                    if (agentCoordinates == targetCoordinates)
                    {
                        await Kill(mission.Target, mission.Agent, mission);
                    }

                    double distans = Calculations.GetDistans(agentCoordinates, targetCoordinates);

                    double timeToUpdate = CalculatTimeLeft(distans);

                    mission.ExecutionTime = timeToUpdate;

                    string directAgentToTarget = DirectAgentToTarget(agentCoordinates, targetCoordinates);

                    Dictionary<string, string> direction = new Dictionary<string, string>();

                    direction.Add("direction", directAgentToTarget);

                    var newCoordinates = UpdateCoordinates.Move(direction, agentCoordinates);

                    mission.Agent.coordinates = newCoordinates;

                    if (agentCoordinates == targetCoordinates)
                    {
                        await Kill(mission.Target, mission.Agent, mission);
                    }
                    
                }
            }
        }

        public bool IsMissionsToUpdate(Mission mission)
        { 
            if (mission.Status == MissionStatus.AssignmentToTask)
            {
                return true;
            }
            return false;
        }

        public double CalculatTimeLeft(double distans)
        {
            return distans / 5;
        }

        public string DirectAgentToTarget(Coordinates agentCoordinates, Coordinates targetCoordinates)
        {
            if (agentCoordinates.X == targetCoordinates.X && targetCoordinates.Y < targetCoordinates.Y)
            {
                return "n";
            }
            if (agentCoordinates.X == targetCoordinates.X && targetCoordinates.Y > targetCoordinates.Y)
            {
                return "s";
            }
            if (agentCoordinates.Y == targetCoordinates.Y && targetCoordinates.X < targetCoordinates.X)
            {
                return "e";
            }
            if (agentCoordinates.Y == targetCoordinates.Y && targetCoordinates.X > targetCoordinates.X)
            {
                return "w";
            }
            if (agentCoordinates.X > targetCoordinates.X && targetCoordinates.Y > targetCoordinates.Y)
            {
                return "sw";
            }
            if (agentCoordinates.X < targetCoordinates.Y && targetCoordinates.X < targetCoordinates.Y)
            {
                return "ne";
            }
            if (agentCoordinates.X < targetCoordinates.X && targetCoordinates.Y > targetCoordinates.Y)
            {
                return "se";
            }
            if (agentCoordinates.X > targetCoordinates.X && targetCoordinates.Y < targetCoordinates.Y)
            {
                return "nw";
            }

            return "";
            
        }

        public async Task Kill(Target target, Agent agent, Mission mission)
        {
            target.Status = TargetStatus.Dead;
            agent.Status = AgentStatus.Sleep;
            mission.Status = MissionStatus.Cmpleted;

            _context.Targets.Update(target);
            _context.Agents.Update(agent);
            _context.Missions.Update(mission);
            await _context.SaveChangesAsync();
        }
    }
}

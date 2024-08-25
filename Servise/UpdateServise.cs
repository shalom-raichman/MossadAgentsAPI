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

                if (!IsMissionsToUpdate(mission)) { continue; }

                var agentCoordinates = mission.Agent.coordinates;
                var targetCoordinates = mission.Target.coordinates;

                if (agentCoordinates.X == targetCoordinates.X && agentCoordinates.Y == targetCoordinates.Y)
                {
                    await Kill(mission.Target, mission.Agent, mission);
                }

                double distans = Calculations.GetDistans(agentCoordinates, targetCoordinates);
                mission.distance = distans;
                _context.Missions.Update(mission);
                await _context.SaveChangesAsync();

                double timeToUpdate = CalculatTimeLeft(distans);

                mission.ExecutionTime = timeToUpdate;

                string directAgentToTarget = DirectAgentToTarget(agentCoordinates, targetCoordinates);
                //if(directAgentToTarget == "") { continue; }

                Dictionary<string, string> direction = new Dictionary<string, string>();

                direction.Add("direction", directAgentToTarget);
                var newCoordinates = UpdateCoordinates.Move(direction, agentCoordinates);

                mission.Agent.coordinates = newCoordinates;

                if (agentCoordinates.X == targetCoordinates.X && agentCoordinates.Y == targetCoordinates.Y)
                {
                    await Kill(mission.Target, mission.Agent, mission);
                }
                _context.Missions.Update(mission);
                _context.Targets.Update(mission.Target);
                _context.Agents.Update(mission.Agent);
                await _context.SaveChangesAsync();

                
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

        public string DirectAgentToTarget(Coordinates agent, Coordinates target)
        {
            if (agent.X == target.X && agent.Y < target.Y)
            {
                return "n";
            }
            if (agent.X == target.X && agent.Y > target.Y)
            {
                return "s";
            }
            //
            if (agent.Y == target.Y && agent.X < target.X)
            {
                return "e";
            }
            if (agent.Y == target.Y && agent.X > target.X)
            {
                return "w";
            }
            //
            if (agent.X > target.X && agent.Y > target.Y)
            {
                return "sw";
            }
            if (agent.X < target.Y && agent.X < target.Y)
            {
                return "ne";
            }
            //
            if (agent.X < target.X && agent.Y > target.Y)
            {
                return "se";
            }
            if (agent.X > target.X && agent.Y < target.Y)
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

using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;
using System.Collections.Generic;
using System.Reflection;

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
            // get all missions from the DB with the agnts and the targets
            var missions = await _context.Missions
                .Include(m => m.Target)
                .ThenInclude(t => t.coordinates)
                .Include(m => m.Agent)
                .ThenInclude(m => m.coordinates)
                .ToListAsync();

            // iterate thrue all the missions
            foreach (var mission in missions)
            {
                // check if mission is null 
                if (mission == null) { continue; }

                // check if the mission need update
                if (!IsMissionsToUpdate(mission)) { continue; }

                // difind the agent and target coordinates
                var agentCoordinates = mission.Agent.coordinates;
                var targetCoordinates = mission.Target.coordinates;

                // check if agnt and target on the same coordinates and commit kill
                if (agentCoordinates.X == targetCoordinates.X && agentCoordinates.Y == targetCoordinates.Y)
                {
                    await Kill(mission.Target, mission.Agent, mission);
                }

                // get the distance of agent from targt
                double distans = Calculations.GetDistans(agentCoordinates, targetCoordinates);

                // update the distance
                mission.distance = distans;
                _context.Missions.Update(mission);
                await _context.SaveChangesAsync();

                // get the time to the kill
                double timeToUpdate = CalculatTimeLeft(distans);

                // set the time to the kill
                mission.ExecutionTime = timeToUpdate;

                // get the agent dirction to go
                string directAgentToTarget = DirectAgentToTarget(agentCoordinates, targetCoordinates);

                // init dict to hold the dirction the agent shuld go
                Dictionary<string, string> direction = new Dictionary<string, string>();
                direction.Add("direction", directAgentToTarget);
                
                // get the new coordinates of the agent
                var newCoordinates = UpdateCoordinates.Move(direction, agentCoordinates);
                
                // set the agent coordinates
                mission.Agent.coordinates = newCoordinates;

                // update the changes in to the DB
                _context.Missions.Update(mission);
                _context.Targets.Update(mission.Target);
                _context.Agents.Update(mission.Agent);
                await _context.SaveChangesAsync();

                
                // check again if the agent got to the target
                if (agentCoordinates.X == targetCoordinates.X && agentCoordinates.Y == targetCoordinates.Y)
                {
                    await Kill(mission.Target, mission.Agent, mission);
                }
            }
        }


        // return bool if mission is to be updated
        public bool IsMissionsToUpdate(Mission mission)
        { 
            if (mission.Status == MissionStatus.AssignmentToTask)
            {
                return true;
            }
            return false;
        }

        // Calculat Time Left to the kill
        public double CalculatTimeLeft(double distans)
        {
            return distans / 5;
        }

        // algorithem that return the direction of the agent to the target
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
            if (agent.Y == target.Y && agent.X < target.X)
            {
                return "e";
            }
            if (agent.Y == target.Y && agent.X > target.X)
            {
                return "w";
            }
            if (agent.X > target.X && agent.Y > target.Y)
            {
                return "sw";
            }
            if (agent.X < target.Y && agent.X < target.Y)
            {
                return "ne";
            }
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

        // kill target and update the DB
        public async Task Kill(Target target, Agent agent, Mission mission)
        {
            target.Status = TargetStatus.Dead;
            agent.Status = AgentStatus.Sleep;
            agent.Kills += 1;
            mission.Status = MissionStatus.Cmpleted;

            _context.Targets.Update(target);
            _context.Agents.Update(agent);
            _context.Missions.Update(mission);
            await _context.SaveChangesAsync();
        }
    }
}

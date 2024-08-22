using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Models;
using System.Diagnostics.Eventing.Reader;

namespace MossadAgentsAPI.Servise
{
    public static class StaffRecommendation
    {
        private static readonly MossadAgentsAPIContext _context;


        private static List<Agent> _agents;
        private static Agent _relevantAgent;
        private static Target _relevantTarget;

        // bool func return if agent is < 200 (targetlocation)
        // distans <= 200

        public static void IsInRange(/*Coordinates targetCoordinates*/)
        {
            //foreach (var agent in _context.Agents)
            //{

            //}
        }

        //public static Mission GetMissionMatch(Mission mission)
        //{
        //    double minDistance = int.MinValue;

        //    foreach (var target in _context.Targets)
        //    {
        //        foreach (var agent in _context.Agents)
        //        {
        //            if (agent != null && target != null)
        //            {
        //                double distans = Calculations.GetDistans(agent.coordinates, target.coordinates);
        //                if (distans < minDistance) { minDistance = distans; }

        //            }
        //        }
        //    }
        //}
    }
}

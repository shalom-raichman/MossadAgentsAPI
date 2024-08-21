using Microsoft.CodeAnalysis;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class UpdateLocation
    {
        public static MapLocation Move(string directions, MapLocation location)
        {
            Dictionary<string, MapLocation> directionDict = MapingDirections.GetDict();

            MapLocation mapLocation = directionDict[directions];
            location.X += mapLocation.X;
            location.Y += mapLocation.Y;

            return location;
        }
    }
}

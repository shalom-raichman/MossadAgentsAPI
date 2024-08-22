using Microsoft.CodeAnalysis;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class UpdateCoordinates
    {
        public static Coordinates Move(Dictionary<string, string> direction, Coordinates coordinates)
        {
            Dictionary<string, Coordinates> directionDict = MapingDirectionsDict.GetDict();

            Coordinates newCoordinates = directionDict[direction["direction"]];

            coordinates.X += newCoordinates.X;
            coordinates.Y += newCoordinates.Y;

            return coordinates;
        }
    }
}

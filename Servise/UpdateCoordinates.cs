using Microsoft.CodeAnalysis;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public class UpdateCoordinates
    {
        public static Coordinates Move(Dictionary<string, string> direction, Coordinates coordinates)
        {
            Dictionary<string, Coordinates> directionDict = MapingDirectionsDict.GetDict();

            string key = direction.Values.First();

            Coordinates newCoordinates = directionDict[key];

            Console.WriteLine("rech to dict");
            coordinates.X += newCoordinates.X;
            coordinates.Y += newCoordinates.Y;

            return coordinates;
        }
    }
}

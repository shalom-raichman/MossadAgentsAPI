using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class MapingDirectionsDict
    {
        public static Dictionary<string, Coordinates> locationDict = new Dictionary<string, Coordinates>();

        public static Dictionary<string, Coordinates> GetDict()
        {
            locationDict.Add("n", new Coordinates(0, 1));
            locationDict.Add("s", new Coordinates(0, -1));
            locationDict.Add("w", new Coordinates(-1, 0));
            locationDict.Add("e", new Coordinates(1, 0));
            locationDict.Add("nw", new Coordinates(-1, 1));
            locationDict.Add("ne", new Coordinates(1, 1));
            locationDict.Add("se", new Coordinates(1, -1));
            locationDict.Add("se", new Coordinates(-1, -1));

            return locationDict;
        }
    }
}

using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class MapingDirections
    {
        public static Dictionary<string, MapLocation> locationDict = new Dictionary<string, MapLocation>();

        public static Dictionary<string, MapLocation> GetDict()
        {
            locationDict.Add("n", new MapLocation(0, 1));
            locationDict.Add("s", new MapLocation(0, -1));
            locationDict.Add("w", new MapLocation(-1, 0));
            locationDict.Add("e", new MapLocation(1, 0));
            locationDict.Add("nw", new MapLocation(-1, 1));
            locationDict.Add("ne", new MapLocation(1, 1));
            locationDict.Add("se", new MapLocation(1, -1));
            locationDict.Add("se", new MapLocation(-1, -1));

            return locationDict;
        }
    }
}

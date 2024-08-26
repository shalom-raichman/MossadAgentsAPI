using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class MapingDirectionsDict
    {
        // dict that hold the direction and coordinates
        private static Dictionary<string, Coordinates> locationDict = new Dictionary<string, Coordinates>();

        // maik shure that the dict did not allready initilize
        private static bool _isInit = false;

        // init dict
        private static void InitDict()
        {
            locationDict.Add("n", new Coordinates(0, 1));
            locationDict.Add("s", new Coordinates(0, -1));
            locationDict.Add("w", new Coordinates(-1, 0));
            locationDict.Add("e", new Coordinates(1, 0));
            locationDict.Add("nw", new Coordinates(-1, 1));
            locationDict.Add("ne", new Coordinates(1, 1));
            locationDict.Add("se", new Coordinates(1, -1));
            locationDict.Add("sw", new Coordinates(-1, -1));
        }

        // return the dict
        public static Dictionary<string, Coordinates> GetDict()
        {
            if (_isInit) {return locationDict;}
            InitDict();
            _isInit = true;
            return locationDict;
        }
    }
}

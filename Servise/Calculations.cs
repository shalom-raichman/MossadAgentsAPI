using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class Calculations
    {
        public static double GetDistans(Coordinates agentCoordinates, Coordinates targetCoordinates)
        {
            double x1 = agentCoordinates.X;
            double y1 = agentCoordinates.Y;
            double x2 = targetCoordinates.X;
            double y2 = targetCoordinates.Y;

            double result =  Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));


            return result;
        }
    }
}

using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class BordersValidation
    {
        public static bool AllowToMove(Agent agent)
        {
            if (agent.coordinates.X > 1000 || agent.coordinates.Y > 1000)
            {
                return false;
            }
            return true;
        }
    }
}

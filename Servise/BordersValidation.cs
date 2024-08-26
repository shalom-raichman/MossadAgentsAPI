using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class BordersValidation
    {
        public static bool IsAgentAllowToMove(Agent agent)
        {
            if (agent == null || agent.coordinates == null) { return false; }
            if (agent.coordinates.X > 1000 || agent.coordinates.Y > 1000)
            {
                return false;
            }
            return true;
        }
    }
}

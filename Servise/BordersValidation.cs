using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Servise
{
    public static class BordersValidation
    {
        public async static Task<bool> AllowToMove(Agent agent)
        {
            if (agent == null) { return false; }
            if (agent.coordinates.X > 1000 || agent.coordinates.Y > 1000)
            {
                return false;
            }
            return true;
        }
    }
}

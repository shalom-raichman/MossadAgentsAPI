using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Models;
using MossadAgentsAPI.Servise;

namespace MossadAgentsAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly MossadAgentsAPIContext _context;
        private readonly AgentMissionServise _missionServise;
        private readonly TargetMissionServise _targetMissionServise;
        private int Id = 1;

        public AgentsController(MossadAgentsAPIContext context, AgentMissionServise agentMissionServise, TargetMissionServise targetMissionServise)
        {
            _context = context;
            _missionServise = agentMissionServise;
            _targetMissionServise = targetMissionServise;
        }


        // GET: api/Agents
        [HttpGet]
        public async Task<IActionResult> GetAgents()
        {

            int status = StatusCodes.Status200OK;
            var Agents = await this._context.Agents.Include(c => c.coordinates).ToListAsync();
            return StatusCode(
                status,
                Agents
                );
        }

        // GET api/Agent/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTAgentById(int id)
        {
            Agent agent = await _context.Agents.FirstOrDefaultAsync(agent => agent.id == id);
            if (agent == null) return NotFound();
            return StatusCode(
            StatusCodes.Status200OK,
            agent
            );
        }

        // POST: create new agent
        [HttpPost]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTarget([FromBody] Agent newAgent)
        {
            if (newAgent == null) return NotFound();
            //int newAgentId = Id;
            //Id += 1;
            //newAgent.id = newAgentId;
            newAgent.Status = Enums.AgentStatus.Sleep;
            await _context.Agents.AddAsync(newAgent);
            await _context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                new { Id =  newAgent.id }
            );
        }

        // PUT api/agents/{id}/pin
        [HttpPut("{id}/pin")]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InitilizeLocation(int id, [FromBody] Coordinates coordinates)
        {
            // Search the target to be update by id
            var agent = await _context.Agents.FirstOrDefaultAsync(agent => agent.id == id);

            // return not found message if not found
            if (agent == null) return NotFound();

            // update the location
            agent.coordinates = coordinates;
            _context.Agents.Update(agent);
            _context.coordinates.Update(agent.coordinates);
            await _context.SaveChangesAsync();

            await _targetMissionServise.DleteUnsportedMissins();

            await _missionServise.CreteMission(agent);

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/agents/{id}/move
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveTarget(int id, [FromBody] Dictionary<string, string> direction)
        {
            if (id == null || direction == null) return NotFound();

            var agent = await _context.Agents.Include(c => c.coordinates).FirstOrDefaultAsync(agent => agent.id == id);

            if (agent == null) return NotFound();

            bool allowToMove = BordersValidation.IsAgentAllowToMove(agent);
            // check if the agent is on mission
            if (agent.Status == Enums.AgentStatus.OnMission) {
                return StatusCode(StatusCodes.Status201Created,
                new { message = "agent is on mission" }
                );
            }
            // check if cen move agent without exit borders
            else if (!allowToMove) 
            {
                return StatusCode(StatusCodes.Status201Created,
                new { message = "cenot move agent outside of borders", currntLocation = agent.coordinates }
                );
            }

            // get the new location to be updated
            Coordinates newCoordinates = UpdateCoordinates.Move(direction, agent.coordinates);

            // update the new location
            agent.coordinates = newCoordinates;
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();

            await _targetMissionServise.DleteUnsportedMissins();

            await _missionServise.CreteMission(agent);

            return StatusCode(StatusCodes.Status201Created,
                new { oldCoordinates = direction, newdirection = newCoordinates }
                );
        }
    }
}

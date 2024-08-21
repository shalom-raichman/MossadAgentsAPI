using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly MossadAgentsAPIContext _context;

        public AgentsController(MossadAgentsAPIContext context)
        {
            _context = context;
        }




        // GET: api/Agents
        [HttpGet]
        public async Task<IActionResult> GetAttacks()
        {
            var attacks = await _context.Agents.ToListAsync();

            return StatusCode(
            StatusCodes.Status200OK,
            attacks
            );
        }

        // GET api/Agents/id
        [HttpGet("{id}")]
        public async Task<IActionResult> AtackStatus(Guid id)
        {
            Agent agent = await _context.Agents.FirstOrDefaultAsync(agent => agent.Id == id);
            if (agent == null) return NotFound();
            return StatusCode(
            StatusCodes.Status200OK,
            agent
            );
        }

        // POST api/<AgentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<AgentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AgentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

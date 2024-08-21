using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;

namespace MossadAgentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly MossadAgentsAPIContext _context;

        public TargetsController(MossadAgentsAPIContext context)
        {
            _context = context;
        }




        // GET: api/Targets
        [HttpGet]
        public async Task<IActionResult> GetTarget()
        {
            var attacks = await _context.Targets.ToListAsync();

            return StatusCode(
            StatusCodes.Status200OK,
            attacks
            );
        }

        // GET api/Targets/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTargetById(Guid id)
        {
            Agent target = await _context.Targets.FirstOrDefaultAsync(Target => Target.Id == id);
            if (target == null) return NotFound();
            return StatusCode(
            StatusCodes.Status200OK,
            target
            );
        }

        // POST: create new target
        [HttpPost]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTarget([FromBody] Target newTarget)
        {
            Guid newTargetId = Guid.NewGuid();
            newTarget.Id = newTargetId;
            await _context.AddAsync(newTarget);
            await _context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                newTargetId
            );
        }

        // PUT api/targets/{id}/pin
        [HttpPut("{id}/pin")]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InitilizeLocation(Guid id, [FromBody] Location location)
        {
            // Search the target to be update by id
            var target = await _context.Targets.FirstOrDefaultAsync(Target => Target.Id == id);

            // return not found message if not found
            if (target == null) return NotFound();
            
            // update the location
            target.Location = location;
            _context.Targets.Update(target);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }


        // PUT api/<AgentsController>/id
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

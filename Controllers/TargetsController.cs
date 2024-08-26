using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;
using MossadAgentsAPI.Servise;

namespace MossadAgentsAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly MossadAgentsAPIContext _context;
        private readonly TargetMissionServise _missionServise;
        private static int Id;

        public TargetsController(MossadAgentsAPIContext context, TargetMissionServise targetMissionServise)
        {
            _context = context;
            _missionServise = targetMissionServise;
        }



        // return all targets
        // GET: api/Targets
        [HttpGet]
        public async Task<IActionResult> GetTarget()
        {

            int status = StatusCodes.Status200OK;
            var targets = await this._context.Targets.Include(c => c.coordinates).ToListAsync();
            return StatusCode(
                status,
                targets
                );
        }

        // return target by id
        // GET api/Targets/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTargetById(int id)
        {
            Target target = await _context.Targets.FirstOrDefaultAsync(Target => Target.Id == id);
            if (target == null) return NotFound();
            return StatusCode(
            StatusCodes.Status200OK,
            target
            );
        }
        // create new target
        // POST: api/targets
        [HttpPost]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTarget([FromBody] Target newTarget)
        {
            if (newTarget == null) return NotFound();
            //int newTargetId = Id;
            //Id += 1;
            //newTarget.Id = newTargetId;
            newTarget.Status = TargetStatus.Alive;
            await _context.Targets.AddAsync(newTarget);
            await _context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                new { Id = newTarget.Id }
            );
        }

        // pin target on the map
        // PUT api/targets/{id}/pin
        [HttpPut("{id}/pin")]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InitilizeLocation(int id, [FromBody] Coordinates location)
        {
            // Search the target to be update by id
            var target = await _context.Targets.FirstOrDefaultAsync(Target => Target.Id == id);

            // return not found message if not found
            if (target == null) return NotFound();
            
            // update the location
            target.coordinates = location;
            _context.Targets.Update(target);
            _context.coordinates.Update(target.coordinates);
            await _context.SaveChangesAsync();

            await _missionServise.DleteUnsportedMissins();

            await _missionServise.CreteMission(target);

            return StatusCode(StatusCodes.Status201Created, 
                new {target.coordinates}
                );
        }

        // move target on the map
        // PUT api/agents/id
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveTarget(int id, [FromBody] Dictionary<string, string> direction)
        {
            if (id == null || direction == null) return NotFound();

            var target = await _context.Targets.Include(c => c.coordinates).FirstOrDefaultAsync(Target => Target.Id == id);

            if(target == null) return NotFound();
            
            // get the new location to be updated
            Coordinates newCoordinates = UpdateCoordinates.Move(direction, target.coordinates);

            // update the new location
            target.coordinates = newCoordinates;
            _context.Targets.Update(target);
            await _context.SaveChangesAsync();

            await _missionServise.DleteUnsportedMissins();

            await _missionServise.CreteMission(target);

            return StatusCode(StatusCodes.Status201Created,
                new { newCoordinates = newCoordinates}
                );
        }
    }
}

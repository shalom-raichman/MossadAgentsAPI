using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Data;
using MossadAgentsAPI.Enums;
using MossadAgentsAPI.Models;
using MossadAgentsAPI.Servise;

namespace MossadAgentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissiionController : ControllerBase
    {
        private readonly MossadAgentsAPIContext _context;
        private readonly UpdateServise _updateServise;
        private readonly SetMissionStatus _setMissionStatus;

        public MissiionController(MossadAgentsAPIContext context, UpdateServise updateServise, SetMissionStatus setMissionStatus)
        {
            _context = context;
            _updateServise = updateServise;
            _setMissionStatus = setMissionStatus;
        }




        // GET: api/Missions
        [HttpGet]
        public async Task<IActionResult> GetMissions()
        {

            int status = StatusCodes.Status200OK;
            var missions = await this._context.Missions.Include(m => m.Agent).ThenInclude(t => t.coordinates).Include(t => t.Target).ThenInclude(t => t.coordinates).ToListAsync();
            return StatusCode(
                status,
                missions
                );
        }

        // GET: api/Targets/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMissionById(int id)
        {
            Mission? mission = await _context.Missions
                .Include(m => m.Target)
                .ThenInclude(t => t.coordinates)
                .Include(m => m.Agent)
                .ThenInclude(a => a.coordinates)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null) return NotFound();

            return StatusCode(
            StatusCodes.Status200OK,
            mission
            );
        }
        
        // PUT: api/Targets/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMissionById(int id)
        {
            Mission mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null) return NotFound();

            await _updateServise.UpdateMission();

            return StatusCode(
            StatusCodes.Status200OK,
            mission
            );
        }
        
        // PUT: api/Targets/id/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetMissionStstusById(int id)
        {
            Mission mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null) return NotFound();

            await _setMissionStatus.SetSttatusToAssignmentToTask(mission);

            return StatusCode(
            StatusCodes.Status200OK,
            mission
            );
        }

        //// POST: create new target
        //[HttpPost]
        //[Produces("Application/json")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //public async Task<IActionResult> CreateTarget([FromBody] Target newTarget)
        //{
        //    if (newTarget == null) return NotFound();
        //    Guid newTargetId = Guid.NewGuid();
        //    newTarget.Id = newTargetId;
        //    newTarget.Status = TargetStatus.Alive;
        //    await _context.Targets.AddAsync(newTarget);
        //    await _context.SaveChangesAsync();
        //    return StatusCode(
        //        StatusCodes.Status201Created,
        //        newTargetId
        //    );
        //}

        //// PUT api/targets/{id}/pin
        //[HttpPut("{id}/pin")]
        //[Produces("Application/json")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> InitilizeLocation(Guid id, [FromBody] Coordinates location)
        //{
        //    // Search the target to be update by id
        //    var target = await _context.Targets.FirstOrDefaultAsync(Target => Target.Id == id);

        //    // return not found message if not found
        //    if (target == null) return NotFound();

        //    // update the location
        //    target.coordinates = location;
        //    _context.Targets.Update(target);
        //    _context.coordinates.Update(target.coordinates);
        //    await _context.SaveChangesAsync();

        //    return StatusCode(StatusCodes.Status201Created, 
        //        new {target.coordinates}
        //        );
        //}

        //// PUT api/agents/id
        //[HttpPut("{id}/move")]
        //public async Task<IActionResult> MoveTarget(Guid id, [FromBody] Dictionary<string, string> direction)
        //{
        //    //if (id == null || direction == null) return NotFound();

        //    var target = await _context.Targets.Include(c => c.coordinates).FirstOrDefaultAsync(Target => Target.Id == id);

        //    if(target == null) return NotFound();

        //    // get the new location to be updated
        //    Coordinates newCoordinates = UpdateCoordinates.Move(direction, target.coordinates);

        //    // update the new location
        //    target.coordinates = newCoordinates;
        //    _context.Targets.Update(target);
        //    await _context.SaveChangesAsync();

        //    return StatusCode(StatusCodes.Status201Created,
        //        new { oldCoordinates = direction, newdirection = newCoordinates}
        //        );
        //}
    }
}

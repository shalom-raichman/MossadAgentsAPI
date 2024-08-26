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
    public class MissionController : ControllerBase
    {
        private readonly MossadAgentsAPIContext _context;
        private readonly UpdateServise _updateServise;
        private readonly SetMissionStatus _setMissionStatus;

        public MissionController(MossadAgentsAPIContext context, UpdateServise updateServise, SetMissionStatus setMissionStatus)
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

        // GET: api/mission/id
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
        
        // PUT: api/Mission/id
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
        
        // POST: api/Update
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateMission()
        {
            await _updateServise.UpdateMission();

            return StatusCode(
            StatusCodes.Status200OK
            );
        }
        
        // PUT: missions/id/status
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

        
    }
}

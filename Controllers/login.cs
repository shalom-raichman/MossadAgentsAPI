using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MossadAgentsAPI.Models;
using NuGet.Common;

namespace MossadAgentsAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class login : ControllerBase
    {
        // POST: create new agent
        [HttpPost]
        [Produces("Application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult CreateLogin(/*[FromBody] Login login*/)
        {
            
            return StatusCode(
                StatusCodes.Status201Created,
                new { token = "435eFDFSD"}
            );
        }
    }
}

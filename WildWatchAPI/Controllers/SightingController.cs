using Microsoft.AspNetCore.Mvc;
using WildWatchAPI.DTOs;
using WildWatchAPI.Services;

namespace WildWatchAPI.Controllers
{
    [Route("sighting")]
    [ApiController]
    public class SightingController : ControllerBase
    {
        private readonly ISightingService _sightingService;

        public SightingController(
            ISightingService sightingService)
        {
            _sightingService = sightingService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateSighting([FromBody] SightingDto s)
        {
            try
            {
                return Ok(await _sightingService.CreateAsync(s));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

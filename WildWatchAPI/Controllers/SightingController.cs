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

        [HttpGet("read/{id}")]
        public async Task<ActionResult> GetSighting([FromRoute] string id)
        {
            try
            {
                return Ok(await _sightingService.GetAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateSighting([FromBody]SightingDto s, [FromQuery] string sightingId )
        {
            try
            {
                return Ok(await _sightingService.UpdateAsync(sightingId, s));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteSighting([FromQuery] string sighingId)
        {
            try
            {
                await _sightingService.DeleteAsync(sighingId);
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getBySighter/{id}")]
        public async Task<ActionResult> GetSightingsBySighter(string id)
        {
            try
            {
                var result = await _sightingService.GetSightingsBySighter(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getMySightings")]
        public async Task<ActionResult> GetMySightings()
        {
            try
            {
                var claims = HttpContext.User.Claims;
                var userId = claims.Where(c => c.Type == "Id").FirstOrDefault()?.Value ?? "-1";

                var result = await _sightingService.GetSightingsBySighter(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

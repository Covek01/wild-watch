using Microsoft.AspNetCore.Mvc;
using WildWatchAPI.DTOs;
using WildWatchAPI.Services;

namespace WildWatchAPI.Controllers
{
    [Route("habitat")]
    [ApiController]
    public class HabitatController : ControllerBase
    {
        private readonly IHabitatService _habitatService;

        public HabitatController(
            IHabitatService habitatService)
        {
            _habitatService = habitatService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateHabitat([FromBody] HabitatDto h) 
        {
            return NotFound("Habitat is created automatically with sightings");
            //try
            //{
            //    var result=await _habitatService.CreateAsync(h);
            //    return Ok(result);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }

        [HttpGet("read/{id}")]
        public async Task<ActionResult> GetHabitat([FromRoute] string id)
        {
            try
            {
                return Ok(await _habitatService.GetAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("readMinSize/{size}")]
        public async Task<ActionResult> GetWithNumberOfSightings([FromRoute] int size)
        {
            try
            {
                return Ok(await this._habitatService.GetWithNumberOfSightings(size));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update")]
        public async Task<ActionResult> UpdateHabitat([FromBody] HabitatDto h, [FromQuery] string habitatId)
        {
            try
            {
                return Ok(await _habitatService.UpdateAsync(habitatId, h));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteHabitat([FromQuery] string habitatId)
        {
            try
            {
                await _habitatService.DeleteAsync(habitatId);
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;
using WildWatchAPI.Services;

namespace WildWatchAPI.Controllers
{
    [Route("species")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly ISpeciesService _speciesService;

        public SpeciesController(
            ISpeciesService speciesService)
        {
            _speciesService = speciesService ?? throw new ArgumentNullException(nameof(speciesService));
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateSpecies([FromBody] SpeciesDto s)
        {
            try
            {
                var result = await _speciesService.CreateAsync(s);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteSpecies([FromQuery] string speciedId)
        {
            try
            {
                await _speciesService.DeleteAsync(speciedId);
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateSpecies([FromBody] SpeciesDto s, [FromQuery] string speciesId)
        {
            try
            {
                return Ok(await _speciesService.UpdateAsync(speciesId, s));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpGet("read/{id}")]
        public async Task<ActionResult> GetSpecies([FromRoute] string id)
        {
            try
            {
                return Ok(await _speciesService.GetAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByCommonName/{name}")]
        public async Task<ActionResult> GetByCommonName([FromRoute] string name)
        {
            try
            {
                return Ok(await _speciesService.GetByCommonName(name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByCommonNameFiltered/{name}")]
        public async Task<ActionResult> GetByCommonNameFiltered([FromRoute] string name)
        {
            try
            {
                return Ok(await _speciesService.GetByCommonNameFiltered(name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

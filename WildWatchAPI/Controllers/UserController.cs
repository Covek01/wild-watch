using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GeoJsonObjectModel;
using WildWatchAPI.DTOs;
using WildWatchAPI.Services;

namespace WildWatchAPI.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("extendToken"), Authorize]
        public ActionResult ExtendToken()
        {
            try
            {
                var result = _userService.ExtendToken();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Registration([FromBody] RegistrationUserDto user)
        {
            try
            {
                var result = await _userService.Registration(user);
                if (result != null)
                    return Ok(result);

                return BadRequest("Service fail");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("verifyMail")]
        public async Task<ActionResult> VerifyAccoount([FromQuery] string email, [FromQuery] string code)
        {
            try
            {
                await _userService.VerifyUser(email, code);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {

            }

        }

        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] LogInUserDto user)
        {
            try
            {
                var userWithToken = await _userService.LogIn(user);
                if (userWithToken != null)
                    return Ok(userWithToken);

                return BadRequest("Service fail");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser([FromQuery] string userId)
        {
            try
            {
                await _userService.DeleteAsync(userId);
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("read/{Id}")]
        public async Task<ActionResult> GetUser([FromRoute] string Id)
        {
            try
            {
                var user = await _userService.GetAsync(Id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto user, [FromQuery] string userId)
        {
            try
            {
                return Ok(await _userService.UpdateAsync(userId, user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getMyLocation")]
        public async Task<ActionResult> GetMyLocation()
        {
            try
            {
                return Ok(await _userService.GetMyLocation());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("setMyLocation")]
        public async Task<ActionResult> SetMyLocation([FromBody] GeoJson2DGeographicCoordinates location)
        {
            try
            {
                await _userService.SetMyLocation(location);
                return Ok("location set");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("addFavouriteSpecies/{id}")]
        public async Task<ActionResult> AddFavouriteSpecies([FromRoute] string id)
        {
            try
            {
                return Ok(await _userService.AddMyFavouriteSpecies(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("removeFavouriteSpecies/{id}")]
        public async Task<ActionResult> RemoveFavouriteSpecies([FromRoute] string id)
        {
            try
            {
                return Ok(await _userService.RemoveMyFavouriteSpecies(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("addFavouriteSpecies2/{id}")]
        public async Task<ActionResult> AddFavouriteSpecies2([FromRoute] string id)
        {
            try
            {
                return Ok(await _userService.AddMyFavouriteSpecies2(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("removeFavouriteSpecies2/{id}")]
        public async Task<ActionResult> RemoveFavouriteSpecies2([FromRoute] string id)
        {
            try
            {
                return Ok(await _userService.RemoveMyFavouriteSpecies2(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpPut("setUserName/{id}/{username}")]
        public async Task<ActionResult> SetUserName(string id, string username)
        {
            try
            {
                await _userService.SetUserName(id, username);
                return Ok("Succesfully changed username to " + username);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getMySightings")]
        public async Task<ActionResult> GetMySightings()
        {
            try
            {
                var result = await _userService.GetMySightings();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getMyFavouriteSpecies")]
        public async Task<ActionResult> GetMyFavouriteSpecies()
        {
            try
            {
                var result = await _userService.GetMyFavouriteSpecies();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("addToMySpecies/{id}")]
        public async Task<ActionResult> AddMyFavouriteSpecies(string id)
        {
            try
            {
                var result = await _userService.AddMyFavouriteSpecies(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("removeFromSpecies/{id}")]
        public async Task<ActionResult> RemoveFromSpecies(string id)
        {
            try
            {
                var result = await _userService.RemoveMyFavouriteSpecies(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getMyFavouriteSpeciesIds")]
        public async Task<ActionResult> GetMyFavouriteSpeciesIds()
        {
            try
            {
                var result = await _userService.GetMyFavouriteSpeciesIds();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getWholeFavouriteSpecies")]
        public async Task<ActionResult> GetMyFavouriteSpeciesWhole()
        {
            try
            {
                var result = await _userService.GetMyFavouriteSpeciesWhole();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

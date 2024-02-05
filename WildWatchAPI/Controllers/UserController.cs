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
                return Ok(await _userService.GetAsync(Id));
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
    }
}

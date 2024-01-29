using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("read/{id}")]
        public async Task<ActionResult> GetUser([FromRoute] string userId)
        {
            try
            {
                return Ok(await _userService.GetAsync(userId));
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
    }
}

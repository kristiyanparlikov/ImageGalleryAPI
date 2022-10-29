using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using photo_gallery_api.Entities;
using photo_gallery_api.Models;
using photo_gallery_api.Repository;

namespace photo_gallery_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<User> result = await _userRepository.GetUsers();
            if (result == null) return NotFound("No users exist in the database");
            return Ok(result);
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<ActionResult<User>> SignIn([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await _userRepository.GetUserByEmail(model.Email);

            if (user == null) return NotFound(new { message = $"An existing record with email '{model.Email}' was not found." });
            return Ok(user);
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<ActionResult<User>> SignUp([FromBody] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User userFromDb = await _userRepository.GetUserByEmail(model.Email);
            if (userFromDb == null)
            {
                User user = new() { Email = model.Email };
                var result = await _userRepository.InsertUser(user);
                return Created("Created user with email: " + result.Email, result);
            }
            else
            {
                return Conflict(new { message = $"An existing record with email '{model.Email}' was already found." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            User result = await _userRepository.GetUserById(id);
            if (result == null)
            {
                return NotFound(new { message = $"An existing record with id '{id}' was not found." });
            }
            await _userRepository.DeleteUser(result);
            return NoContent();

        }
    }
}

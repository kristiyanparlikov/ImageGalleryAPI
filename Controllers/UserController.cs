using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using photo_gallery_api.Entities;
using photo_gallery_api.Repository;

namespace photo_gallery_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<User> result = await _userRepository.GetUsers();
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUser(string email)
        {
            User result = await _userRepository.GetUserByEmail(email);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody]string email)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User userFromDb = await _userRepository.GetUserByEmail(email);
                if(userFromDb == null)
                {
                    User user = new User { Email = email };
                    var result = await _userRepository.InsertUser(user);
                    return Created("Created user with email: " + result.Email, result);
                }
                else
                {
                    return StatusCode(409, "Email is already taken!");
                }
                
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            User result = await _userRepository.GetUserById(id);
            if (result == null)
            {
                return NotFound();
            }
            await _userRepository.DeleteUser(result);
            return NoContent();

        }
    }
}

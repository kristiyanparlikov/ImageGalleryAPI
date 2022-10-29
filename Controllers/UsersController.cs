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
            return Ok(result);
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<ActionResult<User>> SignIn([FromBody] UserModel model)
        {
            User result = await _userRepository.GetUserByEmail(model.Email);

            if (result == null)
            {
                return NotFound("User with such email does not exist!");
            }
            return Ok(result);
        }

        [Route("SignUp")]
        [HttpPost]
        public async Task<ActionResult<User>> SignUp([FromBody]UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User userFromDb = await _userRepository.GetUserByEmail(model.Email);
                if(userFromDb == null)
                {
                    User user = new User { Email = model.Email };
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

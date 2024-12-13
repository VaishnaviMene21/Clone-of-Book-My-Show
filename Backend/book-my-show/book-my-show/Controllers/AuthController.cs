
//using book_my_show.Data;
//using book_my_show.Model;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace book_my_show.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly AppDbContext context;
//        private readonly EncryptionService encryptionService;

//        public AuthController(AppDbContext context, EncryptionService encryptionService)
//        {
//            this.context = context;
//            this.encryptionService = encryptionService;  
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register(User user)
//        {
//            try
//            {
//                var existingUser = await context.Users
//                    .FirstOrDefaultAsync(u => u.Username == user.Username);

//                if (existingUser != null)
//                {
//                    return BadRequest(new { message = "Username already exists." });
//                }

//                user.Password = encryptionService.Encrypt(user.Password);

//                await context.Users.AddAsync(user);
//                await context.SaveChangesAsync();
//                return Ok(new { message = "User registered successfully." });
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while registering the user.", error = ex.Message });
//            }
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] User userLogin)
//        {
//            try
//            {
//                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == userLogin.Username);

//                if (user == null)
//                {
//                    return Unauthorized(new { message = "Invalid username or Password" });
//                }

//                var decryptedPassword = encryptionService.Decrypt(user.Password);
//                if (decryptedPassword != userLogin.Password)
//                {
//                    return Unauthorized(new { message = "Invalid username or Password" });
//                }

//                return Ok(new { message = "Login Successful", username = user.Username });
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during login.", error = ex.Message });
//            }
//        }

//        [HttpGet("get-location/{username}")]
//        public async Task<IActionResult> GetLocation(string username)
//        {
//            try
//            {
//                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

//                if (user == null)
//                {
//                    return NotFound("User not found");
//                }

//                return Ok(new { location = user.Location });
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching the location.", error = ex.Message });
//            }
//        }

//        [HttpPost("update-location")]
//        public async Task<IActionResult> UpdateLocation([FromBody] User user)
//        {
//            try
//            {
//                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

//                if (existingUser == null)
//                {
//                    return NotFound("User not found");
//                }

//                if (!string.IsNullOrEmpty(user.Location))
//                {
//                    existingUser.Location = user.Location;
//                    await context.SaveChangesAsync();
//                }

//                return Ok(existingUser);
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the location.", error = ex.Message });
//            }
//        }
//    }
//}

using book_my_show.Model;
using book_my_show.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace book_my_show.Controllers              
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly EncryptionService encryptionService;
        public AuthController(IUserRepository userRepository, EncryptionService encryptionService)
        {
            _userRepository = userRepository;
            this.encryptionService = encryptionService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (await _userRepository.UserExists(user.Username))
                {
                    return BadRequest(new { message = "Username already exists." });
                }

                await _userRepository.RegisterUser(user);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while registering the user.", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userLogin)
        {
            try
            {
                var user = await _userRepository.GetUserByUsername(userLogin.Username);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or Password" });
                }

                var decryptedPassword = encryptionService.Decrypt(user.Password);
                if (decryptedPassword != userLogin.Password)
                {
                    return Unauthorized(new { message = "Invalid username or Password" });
                }

                return Ok(new { message = "Login Successful", username = user.Username });
            } 
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during login.", error = ex.Message });
            }
        }

        [HttpGet("get-location/{username}")]
        public async Task<IActionResult> GetLocation(string username)
        {
            try
            {
                var location = await _userRepository.GetUserLocation(username);

                if (location == null)
                {
                    return NotFound("User not found");
                }

                return Ok(new { location });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching the location.", error = ex.Message });
            }
        }

        [HttpPost("update-location")]
        public async Task<IActionResult> UpdateLocation([FromBody] User user)
        {
            try
            {
                await _userRepository.UpdateUserLocation(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the location.", error = ex.Message });
            }
        }
    }
} 


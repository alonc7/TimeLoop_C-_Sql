using TimeLoop_Api.Models;
using System;
using System.Web.Http;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using TimeLoop_Api.UserBLL;
using System.Web.Http.Results;

namespace TimeLoop_Api.Controllers
{
    public class UserController : ApiController
    {
        private readonly UserRepository _userRepository;

        // Parameterless constructor for Web API to resolve the controller
        public UserController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            _userRepository = new UserRepository(connectionString);
        }

        // GET: api/User
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_userRepository.GetAllUsers());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError(ex);
            }
        }

        // GET: api/User/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                User user = _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError(ex);
            }
        }

        // POST: api/User
        public IHttpActionResult Post([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                _userRepository.AddUser(user);
                return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError(ex);
            }
        }

        // PUT: api/User/5
        public IHttpActionResult Put(int id, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                User existingUser = _userRepository.GetUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                user.Id = id;
                _userRepository.UpdateUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError(ex);
            }
        }

        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                User existingUser = _userRepository.GetUserById(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                _userRepository.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/User/Signup")]
        public IHttpActionResult Signup([FromBody] SignupRequest signupRequest) //DTO -> Data To Object
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            // Check if the user's email already exists
            if (_userRepository.IsEmailExists(signupRequest.Email))
            {
                return BadRequest("Email already exists.");
            }

            // Hash the user's password before saving
            string hashedPassword = HashPassword(signupRequest.Password);

            // Create the User object without specifying the Id property
            User user = new User
            {
                Email = signupRequest.Email,
                FirstName = signupRequest.FirstName,
                LastName = signupRequest.LastName,
                HashedPassword = hashedPassword,
                Birthdate = signupRequest.Birthdate,
                DateOfCreation = DateTime.Now, // Set the current date and time
                LastLogin = DateTime.Now // Set the current date and time
            };

            _userRepository.AddUser(user);
            return CreatedAtRoute("UserRegistrationRoute", new { id = user.Id }, "ok");
        }
        // POST: api/User/Login
        [HttpPost]
        [Route("api/User/Login")]
        public IHttpActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            // Call the database function to verify the password
            bool isPasswordCorrect = _userRepository.VerifyUserPassword(loginRequest.Email, HashPassword(loginRequest.Password));

            if (isPasswordCorrect)
            {
                // Password verification successful
                // Retrieve the user from the database based on the provided email
                User user = _userRepository.GetUserByEmail(loginRequest.Email);

                // Here, you can return only the necessary user information, excluding the hashed password
                return Ok(new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.Birthdate,
                    user.DateOfCreation,
                    user.LastLogin
                });
            }

            return BadRequest("Incorrect email or password.");
        }


        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString(); 
            } 
        }
    }
}

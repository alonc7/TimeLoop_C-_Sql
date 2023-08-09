using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLoop_Api.Controllers
{
    public class SignupRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
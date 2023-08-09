using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLoop_Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HashedPassword { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
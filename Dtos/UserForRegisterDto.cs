using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required] 
        [StringLength(32, MinimumLength=6, ErrorMessage="Password should be between 6 and 32 characters")]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }
}
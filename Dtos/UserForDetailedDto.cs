using System;

namespace RealEstate.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserPhotoForDetailedDto UserPhoto { get; set; }
        // public string PhotoUrl { get; set; }
    }
}
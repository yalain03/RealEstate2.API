using Microsoft.AspNetCore.Http;

namespace RealEstate.API.Dtos
{
    public class UserPhotoForCreationDto
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
        public IFormFile File { get; set; }
        public int UserId { get; set; }
    }
}
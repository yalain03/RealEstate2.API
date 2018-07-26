using Microsoft.AspNetCore.Http;

namespace RealEstate.API.Dtos
{
    public class PhotosForCreationDto
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
        public IFormFile File { get; set; }
        public int HouseId { get; set; }
    }
}
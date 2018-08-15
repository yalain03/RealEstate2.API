using System.Collections.Generic;

namespace RealEstate.API.Dtos
{
    public class HouseForDetailedDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Rooms { get; set; }
        public double Area { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhotoUrl { get; set; }
        public UserForDetailedDto User { get; set; }
        public IEnumerable<PhotosForDetailedDto> Photos { get; set; }
    }
}
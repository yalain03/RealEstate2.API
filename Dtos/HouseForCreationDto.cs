namespace RealEstate.API.Dtos
{
    public class HouseForCreationDto
    {
        public double Price { get; set; }
        public int Rooms { get; set; }
        public double Area { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Sold { get; set; }
        public int UserId { get; set; }
    }
}
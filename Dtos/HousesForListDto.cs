namespace RealEstate.API.Dtos
{
    public class HousesForListDto
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Rooms { get; set; }
        public double Area { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhotoUrl { get; set; }
    }
}
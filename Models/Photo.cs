namespace RealEstate.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public int HouseId { get; set; }
        public House House { get; set; }
    }
}
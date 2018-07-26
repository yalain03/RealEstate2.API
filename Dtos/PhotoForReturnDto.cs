namespace RealEstate.API.Dtos
{
    public class PhotoForReturnDto
    {
        public int Id { get; set; }
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}
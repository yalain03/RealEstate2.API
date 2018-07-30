namespace RealEstate.API.Helpers
{
    public class UserParams
    {
        public int MaxPageSize { get; set; } = 15;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public double Price { get; set; }
        public double Area { get; set; }
        public int Rooms { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
        
    }
}
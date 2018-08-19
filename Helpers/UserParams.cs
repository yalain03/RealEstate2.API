namespace RealEstate.API.Helpers
{
    public class UserParams
    {
        public int MaxPageSize { get; set; } = 15;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public double Price { get; set; } = -1;
        public double Area { get; set; } = -1;
        public int Rooms { get; set; } = -1;
        public string City { get; set; }
        public string State { get; set; }
        public string Available { get; set; }
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }
        
    }
}
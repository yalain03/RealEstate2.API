using System.Collections.Generic;

namespace RealEstate.API.Models
{
    public class House
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Rooms { get; set; }
        public double Area { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool Sold { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
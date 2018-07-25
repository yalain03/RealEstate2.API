using Microsoft.EntityFrameworkCore;
using RealEstate.API.Models;

namespace RealEstate.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Value> Values { get; set; }
    }
}
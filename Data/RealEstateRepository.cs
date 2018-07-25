using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.API.Models;

namespace RealEstate.API.Data
{
    public class RealEstateRepository : IRealEstateRepository
    {
        private readonly DataContext _context;
        public RealEstateRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.UserPhoto).FirstOrDefaultAsync(u => u.Id == id);

            return user; 
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<House>> GetHouses()
        {
            return await _context.Houses.ToListAsync();
        }
    }
}
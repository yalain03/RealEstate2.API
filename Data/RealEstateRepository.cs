using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.API.Dtos;
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

        public async Task<IEnumerable<House>> GetHouses()
        {
            return await _context.Houses.Where(h => h.Sold == false).ToListAsync();
        }

        public async Task<House> GetHouse(int id)
        {
            var house = await _context.Houses
                .Include(h => h.Photos)
                .FirstOrDefaultAsync(h => h.Id == id);

            return house;
        }

        public async Task<IEnumerable<House>> GetHousesForUser(int userId)
        {
            var houses = await _context.Houses.Include(h => h.Photos).ToListAsync();

            return houses;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }        
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.API.Dtos;
using RealEstate.API.Helpers;
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

        public async Task<PagedList<House>> GetHouses(UserParams userParams)
        {
            var houses = _context.Houses.Where(h => h.Sold == false);

            return await PagedList<House>.CreateAsync(houses, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<House> GetHouse(int id)
        {
            var house = await _context.Houses
                .Include(h => h.Photos)
                .Include(h => h.User)
                .FirstOrDefaultAsync(h => h.Id == id);

            return house;
        }

        public async Task<PagedList<House>> GetHousesForUser(int userId, UserParams userParams)
        {
            var houses = _context.Houses.Where(h => h.UserId == userId);

            return await PagedList<House>.CreateAsync(houses, userParams.PageNumber, userParams.PageSize);
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
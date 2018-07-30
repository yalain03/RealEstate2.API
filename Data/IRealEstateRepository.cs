using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.API.Helpers;
using RealEstate.API.Models;

namespace RealEstate.API.Data
{
    public interface IRealEstateRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
        Task<PagedList<House>> GetHouses(UserParams userParams);
        Task<House> GetHouse(int id);
        Task<PagedList<House>> GetHousesForUser(int userId, UserParams userParams);
        Task<Photo> GetPhoto(int id);
    }
}
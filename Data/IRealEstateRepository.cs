using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstate.API.Models;

namespace RealEstate.API.Data
{
    public interface IRealEstateRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
        Task<IEnumerable<House>> GetHouses();
        Task<House> GetHouse(int id);
        Task<IEnumerable<House>> GetHousesForUser(int userId);
        Task<Photo> GetPhoto(int id);
    }
}
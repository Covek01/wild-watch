using MongoDB.Driver;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public interface IHabitatService
    {
        public Task<string> CreateAsync(IClientSessionHandle session,HabitatDto h);
        public Task<Habitat> GetAsync(string habitatId);
        public Task<List<Habitat>> GetWithNumberOfSightings(int numberOfSightings);
        public Task<Habitat> UpdateAsync(string habitatId, HabitatDto h);
        public Task DeleteAsync(string habitatId);
        public Task SessionDeleteAsync(IClientSessionHandle session, string habitatId);
        //public Task ConsolidateAsync(string habitatId); 
    }
}

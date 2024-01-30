using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public interface ISightingService
    {
        public Task<string> CreateAsync(SightingDto s);

        public Task<Sighting> GetAsync(string sightingId);

        public Task DeleteAsync(string sightingId);

        public Task<Sighting> UpdateAsync(string sightingsId, SightingDto s);
    }
}

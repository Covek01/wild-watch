using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public interface ISpeciesService
    {
        public Task<string> CreateAsync(SpeciesDto s);

        public Task DeleteAsync(string speciesId);

        public Task<Species> UpdateAsync(string speciesId,SpeciesDto s);

        public Task<Species> GetAsync(string speciesId);

        public Task<Species> GetByCommonName(string commonName);
        public Task<List<SpeciesWithIdDto>> GetByCommonNameFiltered(string commonName);
    }
}

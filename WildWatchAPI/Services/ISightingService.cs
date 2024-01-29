using WildWatchAPI.DTOs;

namespace WildWatchAPI.Services
{
    public interface ISightingService
    {

        public Task<string> CreateAsync(SightingDto s);
    }
}

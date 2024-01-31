using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GeoJsonObjectModel;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public interface IUserService
    {
        string ExtendToken();
        Task<User> Registration([FromBody] RegistrationUserDto user);
        Task VerifyUser(string email, string verificationCode);
        Task<UserDto> LogIn([FromBody] LogInUserDto user);

        public Task DeleteAsync(string userId);

        public Task<User> GetAsync(string userId);

        public Task<User> UpdateAsync(string userId, UserUpdateDto user);

        public Task<User> AddMyFavouriteSpecies(string speciesId);
        public Task<User> RemoveMyFavouriteSpecies(string speciesId);
        public Task SetMyLocation(GeoJson2DGeographicCoordinates? location);
        public Task<GeoJson2DGeographicCoordinates?> GetMyLocation();

    }
}

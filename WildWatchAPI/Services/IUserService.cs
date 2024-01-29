using Microsoft.AspNetCore.Mvc;
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
    }
}

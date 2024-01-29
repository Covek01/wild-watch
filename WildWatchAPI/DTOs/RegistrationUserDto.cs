namespace WildWatchAPI.DTOs
{
    public class RegistrationUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }


    }
}

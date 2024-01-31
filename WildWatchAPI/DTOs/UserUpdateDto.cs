using MongoDB.Driver.GeoJsonObjectModel;
using WildWatchAPI.Models;

namespace WildWatchAPI.DTOs
{
    public class UserUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool AccountConfirmed { get; set; }

        public string ImageUrl { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public GeoJson2DGeographicCoordinates? Location { get; set; }

        public UserUpdateDto(User u)
        {
            this.Name = u.Name;
            this.Email = u.Email;
            this.AccountConfirmed = u.AccountConfirmed;
            this.ImageUrl = u.ImageUrl;
            this.DateOfBirth = u.DateOfBirth;
            this.Location = u.Location;
        }

        public UserUpdateDto()
        {

        }
    }
}

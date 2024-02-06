using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Text.Json.Serialization;

namespace WildWatchAPI.Models
{
    public class UserSummary
    {
        public string  userId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int NumberOfSightings { get; set; }
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRequired]
        public string Name { get; set; } = null!;
        [BsonRequired]
        public string Email { get; set; }= null!;
        [BsonRequired]
        public bool AccountConfirmed { get; set; }

        [BsonRequired]
        public string VeficitationCode { get; set; }=null!;
        [BsonRequired]
        [JsonIgnore]
        public string PasswordHash { get; set; }= null!;
        public string ImageUrl { get; set; }= null!;
        public DateTime DateOfBirth { get; set; }
        public GeoJson2DGeographicCoordinates? Location { get; set; }
        //[JsonIgnore]
        public List<SightingSummaryUser> Sightings { get; set; } = new();
        //[JsonIgnore]
        public List<MongoDBRef> FavouriteSpecies { get; set; } = new();
    }

  
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Text.Json.Serialization;

namespace WildWatchAPI.Models
{
    public class SightingSummaryUser
    {
        public MongoDBRef Id { get; set; } = null!;
        public DateTime SightingTime { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; } = null!;
        public SpeciesSummary Species { get; set; }=null!;
    }

    public class SightingSummarySpecies
    {
        public MongoDBRef Id { get; set; } = null!;
        public DateTime SightingTime { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; } = null!;

        public UserSummary Sighter { get; set; } = null!;
    }

    public class SightingSummaryHabitat
    {
        public MongoDBRef Id { get; set; } = null!;
        public DateTime SightingTime { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; } = null!;
        public SpeciesSummary Species { get; set; } = null!;

        public UserSummary Sighter { get; set; } = null!;
    }

    public class Sighting
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime SightingTime { get; set; }


        public GeoJson2DGeographicCoordinates Location { get; set; } = null!;
        [JsonIgnore]
        public UserSummary Sighter { get; set; } = null!;

        [JsonIgnore]
        public SpeciesSummary Species { get; set; } = null!;
        public string? ImageUrl { get; set; }

        [BsonExtraElements]
        public BsonDocument? AdditionalInformation { get; set; }

    }
}

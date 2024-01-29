using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace WildWatchAPI.Models
{
    public class HabitatSummary
    {
        public MongoDBRef Id { get; set; } = null!;

        public List<GeoJsonPoint<GeoJson2DGeographicCoordinates>> BorderPoints { get; set; } = new();
    }
    public class Habitat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public List<GeoJsonPoint<GeoJson2DGeographicCoordinates>> BorderPoints { get; set; } = new();
        public List<SightingSummayHabitat> Sightings { get; set; } = new();
    }
}

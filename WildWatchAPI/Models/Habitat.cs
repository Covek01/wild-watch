using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Text.Json.Serialization;

namespace WildWatchAPI.Models
{
    public class HabitatSummary
    {
        public string habitatId { get; set; } = null!;

        //public List<GeoJsonPoint<GeoJson2DGeographicCoordinates>> BorderPoints { get; set; } = new();
    }
    public class Habitat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        //public List<GeoJsonPoint<GeoJson2DGeographicCoordinates>> BorderPoints { get; set; } = new List<GeoJsonPoint<GeoJson2DGeographicCoordinates>>();
        //[JsonIgnore]
        public List<SightingSummaryHabitat> Sightings { get; set; } = new();
    }
}

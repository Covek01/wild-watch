using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using WildWatchAPI.Models;

namespace WildWatchAPI.DTOs
{
    public class SpeciesWithIdDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Klasa SpeciesClass { get; set; }
        public string CommonName { get; set; } = null!;
        public string ScientificName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ConservationStatus { get; set; } = null!;
    }
}

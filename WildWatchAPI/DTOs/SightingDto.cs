using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.GeoJsonObjectModel;
using WildWatchAPI.Models;

namespace WildWatchAPI.DTOs
{
    public class SightingDto
    {
        public DateTime SightingTime { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; } = null!;
        public string? PhotoUrl { get; set; }

        public string SighterId { get; set; } = null!;

        public Klasa SpeciesClass { get; set; }
        public string CommonName { get; set; } = null!;

        public string ScientificName { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;

        public string ConservationStatus { get; set; }= null!;

        public string? Comment { get; set; }

    }
}

using MongoDB.Driver.GeoJsonObjectModel;
using WildWatchAPI.Models;

namespace WildWatchAPI.DTOs
{
    public class HabitatDto
    {
        public List<GeoJsonPoint<GeoJson2DGeographicCoordinates>> BorderPoints { get; set; } = new();

        public List<SightingSummaryHabitat> Sightings { get; set; } =new();
    }
}

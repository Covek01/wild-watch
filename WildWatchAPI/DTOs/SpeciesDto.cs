using WildWatchAPI.Models;

namespace WildWatchAPI.DTOs
{
    public class SpeciesDto
    {
        public Klasa Class { get; set; }
        public string CommonName { get; set; } = null!;
        public string ScientificName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ConsertvationStatus { get; set; }= null!;

    }
}

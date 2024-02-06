using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text.Json.Serialization;

namespace WildWatchAPI.Models
{
    public class SpeciesSummary
    {
        public string speciesId { get; set; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Klasa Class { get; set; }
        public string CommonName { get; set; } = null!;
        public string ScientificName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ConservationStatus { get; set; } = null!;
    }
    public class Species
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Klasa Class { get; set; }
        public string CommonName { get; set; } = null!;
        public string ScientificName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; }= null!;
        public string ConservationStatus { get; set; }= null!;

        //[JsonIgnore]
        public List<SightingSummarySpecies> Sightings { get; set; } = new();

        //[JsonIgnore]
        public List<HabitatSummary> Habitats { get; set; } = new();


    }
    public enum Klasa
    {
        Sponge,  //sundjeri
        Cnidaria, // dupljari
        Platyhelminthes,//pljosnati crvi
        Annelida, //clankoviti crvi
        Mollusca, //mekusci
        Cephalopoda,//glavonosci
        Arthropoda, //zglavkari
        Echinodermata, //bodljokosci
        Tunicata, //plastasi
        Cephalochordata, //kopljasi
        Fish, //ribe
        Amphibians, //vodozemci
        Reptilia, //gmizavci
        Birds, //ptice
        Mammals// sisari
    }
}

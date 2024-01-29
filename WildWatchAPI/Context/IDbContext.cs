using MongoDB.Driver;
using WildWatchAPI.Models;

namespace WildWatchAPI.Context
{
    public interface IDbContext
    {
        IMongoDatabase DB { get; }
        IMongoClient MongoClient { get; }

        IMongoCollection<User> Users { get; set; }
        IMongoCollection<Species> Species { get; set; }
        IMongoCollection<Sighting> Sightings { get; set; }
        IMongoCollection<Habitat> Habitats { get; set; }

        IMongoCollection<T> GetCollection<T>(string name);
    }
}

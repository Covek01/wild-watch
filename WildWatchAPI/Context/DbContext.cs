using MongoDB.Driver;
using System.Reflection.PortableExecutable;
using WildWatchAPI.Models;

namespace WildWatchAPI.Context
{
    public class DbContext : IDbContext
    {
        public readonly IMongoDatabase _db;
        public readonly IMongoClient _mongoClient;

        public IMongoDatabase DB { get { return _db; } }

        public IMongoClient MongoClient {  get { return _mongoClient; } }

        public IMongoCollection<User> Users { get; set; }
        public IMongoCollection<Species> Species { get;set; }
        public IMongoCollection<Sighting> Sightings { get; set; }
        public IMongoCollection<Habitat> Habitats { get; set; }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public DbContext(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _db = _mongoClient.GetDatabase("WildWatch");

            Users = GetCollection<User>("Users");
            Species = GetCollection<Species>("Species");
            Sightings = GetCollection<Sighting>("Sightings");
            Habitats = GetCollection<Habitat>("Habitats");
        }
    }
}

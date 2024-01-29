using MongoDB.Driver;
using WildWatchAPI.Context;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public class SightingService : ISightingService
    {
        private readonly IDbContext _context;
        private readonly ISpeciesService _speciesService;
        public SightingService(
            IDbContext context,
            ISpeciesService speciesService)
        {
            _context = context;
            _speciesService = speciesService;
        }


        ///TODO:: Add sighting in habitat, kad se razvije logika za habitat
        public async Task<string> CreateAsync(SightingDto s)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var species = await _context.Species.Find(sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
                var speciesId = species.Id ?? "";
                if (species == default)
                {
                    var newSpecies = new SpeciesDto()
                    {
                        Class = s.SpeciesClass,
                        CommonName = s.CommonName,
                        ScientificName = s.ScientificName,
                        ImageUrl = s.PhotoUrl ?? "",
                        Description = s.Description,
                        ConsertvationStatus = s.ConservationStatus
                    };

                    speciesId = await _speciesService.CreateAsync(newSpecies);
                    species = await _context.Species.Find(sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
                }

                var speciesSummary = new SpeciesSummary()
                {
                    Class = species.Class,
                    CommonName = s.CommonName,
                    ScientificName = s.ScientificName,
                    ImageUrl = species.ImageUrl??"",
                    Description = species.Description,
                    ConservationStatus = species.ConservationStatus,
                    Id = new MongoDBRef("Species", speciesId)
                };

                var user = await _context.Users.Find(u => u.Id == s.SighterId).FirstOrDefaultAsync();
                if (user == default)
                {
                    throw new Exception("Invalid User Id");
                }

                var userSummary = new UserSummary()
                {
                    Email = user.Email,
                    Name = user.Name,
                    NumberOfSightings = user.Sightings.Count() + 1,
                    Id = new MongoDBRef("Users", user.Id)
                };

                var sighting = new Sighting()
                {
                    SightingTime = s.SightingTime,
                    Location = s.Location,
                    ImageUrl = s.PhotoUrl ?? "",
                    Sighter = userSummary,
                    Species = speciesSummary
                };
                await _context.Sightings.InsertOneAsync(sighting);


                var sightingSummaryUser = new SightingSummaryUser()
                {
                    Id = new MongoDBRef("Sightings", sighting.Id),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Species = speciesSummary
                };
                var userFilter = Builders<User>.Filter.Where(u => u.Id == user.Id);
                var userUpdate = Builders<User>.Update.Push(u => u.Sightings, sightingSummaryUser);
                await _context.Users.UpdateOneAsync(userFilter, userUpdate);


                var sightingSummarySpecies = new SightingSummarySpecies()
                {
                    Id = new MongoDBRef("Sightings", sighting.Id),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Sighter = userSummary
                };
                var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
                var speciesUpdate = Builders<Species>.Update.Push(sp => sp.Sightings, sightingSummarySpecies);
                await _context.Species.UpdateOneAsync(speciesFilter, speciesUpdate);


                return sighting.Id;
            }
            catch(Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Sighting failed");
            }
        }
    }
}

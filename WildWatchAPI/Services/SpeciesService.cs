using MongoDB.Bson;
using MongoDB.Driver;
using WildWatchAPI.Context;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;
using ZstdSharp.Unsafe;

namespace WildWatchAPI.Services
{
    public class SpeciesService : ISpeciesService
    {
        public readonly IDbContext _context;
        private readonly IHabitatService _habitatService;
        public SpeciesService(
            IDbContext context,
            IHabitatService habitatService) 
        {
            _context = context;
            _habitatService= habitatService;
        }

        public async Task<string> CreateAsync(SpeciesDto s)
        {
            Species species = new Species()
            {
                Class = s.Class,
                CommonName = s.CommonName,
                Description = s.Description,
                ConservationStatus = s.ConsertvationStatus,
                ImageUrl = s.ImageUrl,
                ScientificName = s.ScientificName,
                Habitats = new List<HabitatSummary>(),
                Sightings = new List<SightingSummarySpecies>()
            };
            await _context.Species.InsertOneAsync(species);
            return species.Id;
        }


        public async Task DeleteAsync(string speciesId)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            try
            {
                session.StartTransaction();
                var species = Builders<Species>.Filter.Where(s => s.Id == speciesId);
                
                var sightings=Builders<Sighting>.Filter.Where(s=>s.Species.speciesId==speciesId);

                var habitatsToUpdate = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings,Builders<SightingSummaryHabitat>.Filter.Where(s => s.Species.speciesId == speciesId));
                var habitatsUpdate = Builders<Habitat>.Update.PullFilter(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s=>s.Species.speciesId == speciesId));

                var userFilterFavourite = Builders<User>.Filter.ElemMatch(u => u.FavouriteSpecies, Builders<MongoDBRef>.Filter.Where(s => s.Id == speciesId));
                var userUpdateFavourite = Builders<User>.Update.PullFilter(u => u.FavouriteSpecies, Builders<MongoDBRef>.Filter.Where(s => s.Id == speciesId));

                var userFilterSightings= Builders<User>.Filter.ElemMatch(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Species.speciesId == speciesId));
                var userUpdateSightings = Builders<User>.Update.PullFilter(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Species.speciesId == speciesId));

                await _context.Users.UpdateManyAsync(session,userFilterSightings, userUpdateSightings);
                await _context.Users.UpdateManyAsync(session, userFilterFavourite, userUpdateFavourite);
                await _context.Habitats.UpdateManyAsync(session, habitatsToUpdate, habitatsUpdate);
                await _context.Species.DeleteOneAsync(session, species);
                await _context.Sightings.DeleteManyAsync(session, sightings);

                var emptyHabitatsFilter = Builders<Habitat>.Filter.Where(h => h.Sightings.Count == 0);
                var emptyHabitats = await _context.Habitats.Find(session, emptyHabitatsFilter).ToListAsync();
                foreach (var habitat in emptyHabitats)
                {
                    await _habitatService.DeleteAsync(habitat.Id);
                }

                await session.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Species service failed");
            }
       
        }

        public async Task<Species> UpdateAsync(string speciesId,SpeciesDto s)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            try
            {
                session.StartTransaction();
                var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
                var speciesUpdate = Builders<Species>.Update
                    .Set(sp => sp.Class, s.Class)
                    .Set(sp => sp.CommonName, s.CommonName)
                    .Set(sp => sp.ScientificName, s.ScientificName)
                    .Set(sp => sp.ImageUrl, s.ImageUrl)
                    .Set(sp => sp.Description, s.Description)
                    .Set(sp => sp.ConservationStatus, s.ConsertvationStatus);
                await _context.Species.UpdateOneAsync(session, speciesFilter, speciesUpdate);

                var speciesSummary = new SpeciesSummary()
                {
                    Class = s.Class,
                    CommonName = s.CommonName,
                    ConservationStatus = s.ConsertvationStatus,
                    Description = s.Description,
                    ScientificName = s.ScientificName,
                    ImageUrl = s.ImageUrl,
                    speciesId=  speciesId
                };

                var sightingsFilter = Builders<Sighting>.Filter.Where(sig => sig.Species.speciesId == speciesId);
                var sightingsUpdate = Builders<Sighting>.Update.Set(sig => sig.Species, speciesSummary);
                await _context.Sightings.UpdateManyAsync(session, sightingsFilter, sightingsUpdate);

                var habitatsToUpdate = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Species.speciesId == speciesId));
                var habitatsUpdate = Builders<Habitat>.Update.Set("Sightings.$[h].Species", speciesSummary);
                var habitatArrayFilters = new List<ArrayFilterDefinition>
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("h.Species.speciesId",speciesId))
                };

                var habitatUpdateOptions = new UpdateOptions { ArrayFilters = habitatArrayFilters };
                await _context.Habitats.UpdateManyAsync(session, habitatsToUpdate, habitatsUpdate,habitatUpdateOptions);

                var userFilterSightings = Builders<User>.Filter.ElemMatch(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Species.speciesId == speciesId));
                var userUpdateSightings = Builders<User>.Update.Set("Sightings.$[u].Species", speciesSummary);
                var userArrayFilters = new List<ArrayFilterDefinition>
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("u.Species.speciesId",speciesId))
                };

                var userUpdateOptions = new UpdateOptions { ArrayFilters = userArrayFilters };
                await _context.Users.UpdateManyAsync(session, userFilterSightings, userUpdateSightings,userUpdateOptions);

                var speciesFromDatabase= await _context.Species.Find(session, speciesFilter).FirstOrDefaultAsync();
                await session.CommitTransactionAsync();
                return speciesFromDatabase;
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Species service failed");
            }
        }

        public async Task<Species> GetAsync(string speciesId)
        {
            var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
            var species= await _context.Species.Find(speciesFilter).FirstOrDefaultAsync();
            if (species == default)
            {
                throw new Exception("Invalid Id");
            }
            return species;
        }

        public async Task<Species> GetByCommonName(string commonName)
        {
            var speciesFilter = Builders<Species>.Filter.Where(sp => sp.CommonName.ToLower()== commonName.ToLower());
            var species=await _context.Species.Find(speciesFilter).FirstOrDefaultAsync();
            if(species == default) 
            {
                throw new Exception("Invalid Id");
            }
            return species;
        }

    }
}

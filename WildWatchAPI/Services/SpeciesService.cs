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

        public SpeciesService(
            IDbContext context) 
        {
            _context = context;
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
            session.StartTransaction();
            try
            {
                var species = Builders<Species>.Filter.Where(s => s.Id == speciesId);
                
                var sightings=Builders<Sighting>.Filter.Where(s=>s.Species.Id==new MongoDBRef("Species", speciesId));

               

                var habitatsToUpdate = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings,Builders<SightingSummayHabitat>.Filter.Where(s => s.Species.Id == new MongoDBRef("Species", speciesId)));
                var habitatsUpdate = Builders<Habitat>.Update.PullFilter(h => h.Sightings, Builders<SightingSummayHabitat>.Filter.Where(s=>s.Species.Id==new MongoDBRef("Species", speciesId)));

                var userFilterFavourite = Builders<User>.Filter.ElemMatch(u => u.FavouriteSpecies, Builders<MongoDBRef>.Filter.Where(s => s.Id == speciesId));
                var userUpdateFavourite = Builders<User>.Update.PullFilter(u => u.FavouriteSpecies, Builders<MongoDBRef>.Filter.Where(s => s.Id == speciesId));

                var userFilterSightings= Builders<User>.Filter.ElemMatch(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Species.Id == new MongoDBRef("Species",speciesId)));
                var userUpdateSightings = Builders<User>.Update.PullFilter(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Species.Id == new MongoDBRef("Species", speciesId)));

                await _context.Users.UpdateManyAsync(userFilterSightings, userUpdateSightings);
                await _context.Users.UpdateManyAsync(userFilterFavourite, userUpdateFavourite);
                await _context.Habitats.UpdateManyAsync(habitatsToUpdate, habitatsUpdate);
                await _context.Species.DeleteOneAsync(species);
                await _context.Sightings.DeleteManyAsync(sightings);

                await session.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await session.AbortTransactionAsync();
            }
       
        }

        public async Task<Species> UpdateAsync(string speciesId,SpeciesDto s)
        {
            
            var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
            var speciesUpdate = Builders<Species>.Update
                .Set(sp => sp.Class, s.Class)
                .Set(sp => sp.CommonName, s.CommonName)
                .Set(sp => sp.ScientificName, s.ScientificName)
                .Set(sp => sp.ImageUrl, s.ImageUrl)
                .Set(sp => sp.Description, s.Description)
                .Set(sp => sp.ConservationStatus, s.ConsertvationStatus);
            await _context.Species.UpdateOneAsync(speciesFilter, speciesUpdate);

            return await _context.Species.Find(speciesFilter).FirstOrDefaultAsync();
        }

        public async Task<Species> GetAsync(string speciesId)
        {
            var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
            return await _context.Species.Find(speciesFilter).FirstOrDefaultAsync();
        }

        public async Task<Species> GetByCommonName(string commonName)
        {
            var speciesFilter = Builders<Species>.Filter.Where(sp => sp.CommonName.ToLower()== commonName.ToLower());
            return await _context.Species.Find(speciesFilter).FirstOrDefaultAsync();
        }

    }
}

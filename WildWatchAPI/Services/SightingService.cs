using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using WildWatchAPI.Context;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public class SightingService : ISightingService
    {
        private readonly IDbContext _context;
        private readonly ISpeciesService _speciesService;
        private readonly IHabitatService _habitatService;
        public SightingService(
            IDbContext context,
            ISpeciesService speciesService,
            IHabitatService habitatService)
        {
            _context = context;
            _speciesService = speciesService;
            _habitatService = habitatService;
        }

        public async Task<string> CreateAsync(SightingDto s)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            try
            {
                session.StartTransaction();
                var species = await _context.Species.Find(session,sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
                var speciesId = species == null ? "" : species.Id;
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
                    species = await _context.Species.Find(session, sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
                }

                var speciesSummary = new SpeciesSummary()
                {
                    Class = species.Class,
                    CommonName = s.CommonName,
                    ScientificName = s.ScientificName,
                    ImageUrl = species.ImageUrl??"",
                    Description = species.Description,
                    ConservationStatus = species.ConservationStatus,
                    Id = new MongoDBRef("Species", new ObjectId(speciesId))
                };

                var user = await _context.Users.Find(session,u => u.Id == s.SighterId).FirstOrDefaultAsync();
                if (user == default)
                {
                    throw new Exception("Invalid User Id");
                }

                var userSummary = new UserSummary()
                {
                    Email = user.Email,
                    Name = user.Name,
                    NumberOfSightings = user.Sightings.Count() + 1,
                    Id = new MongoDBRef("Users", new ObjectId(user.Id))
                };

                var sighting = new Sighting()
                {
                    SightingTime = s.SightingTime,
                    Location = s.Location,
                    ImageUrl = s.PhotoUrl ?? "",
                    Sighter = userSummary,
                    Species = speciesSummary
                };
                await _context.Sightings.InsertOneAsync(session, sighting);


                var sightingSummaryUser = new SightingSummaryUser()
                {
                    Id = new MongoDBRef("Sightings", new ObjectId(sighting.Id)),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Species = speciesSummary
                };
                var userFilter = Builders<User>.Filter.Where(u => u.Id == user.Id);
                var userUpdate = Builders<User>.Update.Push(u => u.Sightings, sightingSummaryUser);
                await _context.Users.UpdateOneAsync(session, userFilter, userUpdate);


                var sightingSummarySpecies = new SightingSummarySpecies()
                {
                    Id = new MongoDBRef("Sightings", new ObjectId(sighting.Id)),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Sighter = userSummary
                };
                var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
                var speciesUpdate = Builders<Species>.Update.Push(sp => sp.Sightings, sightingSummarySpecies);
                await _context.Species.UpdateOneAsync(session, speciesFilter, speciesUpdate);

                HabitatDto habitatDto = new HabitatDto()
                {
                    BorderPoints = new List<GeoJsonPoint<GeoJson2DGeographicCoordinates>>(),
                    Sightings = new List<SightingSummaryHabitat>()
                };
                var point = GeoJson.Point(new GeoJson2DGeographicCoordinates(s.Location.Longitude, s.Location.Latitude));
                habitatDto.BorderPoints.Add(point);
                var sightingSummaryHabitat = new SightingSummaryHabitat()
                {
                    Id = new MongoDBRef("Sightings", new ObjectId(sighting.Id)),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Sighter = userSummary,
                    Species = speciesSummary
                };
                habitatDto.Sightings.Add(sightingSummaryHabitat);
                await _habitatService.CreateAsync(habitatDto);

                return sighting.Id;
            }
            catch(Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Sighting failed");
            }
        }

        public async Task<Sighting> GetAsync(string sightingId)
        {
            var sighting = await _context.Sightings.Find(s => s.Id == sightingId).FirstOrDefaultAsync();
            if (sighting == default) 
            {
                throw new Exception("Invalid Id");
            }
            return sighting;
        }

        public async Task DeleteAsync(string sightingId)
        {
            using var session=await _context.MongoClient.StartSessionAsync();
            try
            {
                session.StartTransaction();
                var sighting = Builders<Sighting>.Filter.Where(s => s.Id == sightingId);

                var userSightingsFilter = Builders<User>.Filter.ElemMatch(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", new ObjectId(sightingId))));
                var userSightingsUpdate = Builders<User>.Update.PullFilter(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", new ObjectId(sightingId))));

                var habitatSightingsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", new ObjectId(sightingId))));
                var habitatSightingsUpdate = Builders<Habitat>.Update.PullFilter(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", new ObjectId(sightingId))));

                var speciesSightingsFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Id == new MongoDBRef("Sightings", new ObjectId(sightingId))));
                var speciesSightingsUpdate = Builders<Species>.Update.PullFilter(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Id == new MongoDBRef("Sightings", new ObjectId(sightingId))));


                await _context.Sightings.DeleteOneAsync(session, sighting);
                await _context.Users.UpdateManyAsync(session, userSightingsFilter, userSightingsUpdate);
                await _context.Habitats.UpdateManyAsync(session, habitatSightingsFilter, habitatSightingsUpdate);
                await _context.Species.UpdateManyAsync(session, speciesSightingsFilter, speciesSightingsUpdate);

                var emptyHabitatsFilter = Builders<Habitat>.Filter.Where(h => h.Sightings.Count == 0);
                var emptyHabitats = await _context.Habitats.Find(session, emptyHabitatsFilter).ToListAsync();
                foreach (var habitat in emptyHabitats)
                {
                    await _habitatService.DeleteAsync(habitat.Id);
                }

                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Sighting service failed");
            }
        }

        //ako je user promenjen treba da se skine sa user-a i da se stavi na drugog
        //ako je lokacija promenjena treba i habitat da se promeni
        // ako je vrsta promenjena treba da se skine sa te vrste i stavi na drugu
        public async Task<Sighting> UpdateAsync(string sightingsId,SightingDto s)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            try
            {
                session.StartTransaction();
                var species = await _context.Species.Find(session,sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
                var speciesId = species == null ? "" : species.Id;
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
                    species = await _context.Species.Find(session,sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
                }

                var speciesSummary = new SpeciesSummary()
                {
                    Class = species.Class,
                    CommonName = s.CommonName,
                    ScientificName = s.ScientificName,
                    ImageUrl = species.ImageUrl ?? "",
                    Description = species.Description,
                    ConservationStatus = species.ConservationStatus,
                    Id = new MongoDBRef("Species", speciesId)
                };

                var user = await _context.Users.Find(session,u => u.Id == s.SighterId).FirstOrDefaultAsync();
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

                //var sighting = new Sighting()
                //{
                //    Id=sightingsId,
                //    SightingTime = s.SightingTime,
                //    Location = s.Location,
                //    ImageUrl = s.PhotoUrl ?? "",
                //    Sighter = userSummary,
                //    Species = speciesSummary
                //};
                //await _context.Sightings.ReplaceOneAsync(session,sightingsId, sighting);
                var sightingFilter = Builders<Sighting>.Filter.Where(sig=>sig.Id==sightingsId);
                var sightingUpdate = Builders<Sighting>.Update
                    .Set(s => s.SightingTime, s.SightingTime)
                    .Set(s => s.Location, s.Location)
                    .Set(s => s.ImageUrl, s.PhotoUrl ?? "")
                    .Set(s => s.Sighter, userSummary)
                    .Set(s => s.Species, speciesSummary);
                await _context.Sightings.UpdateOneAsync(session, sightingFilter, sightingUpdate);

                var sightingSummaryUser = new SightingSummaryUser()
                {
                    Id = new MongoDBRef("Sightings", sightingsId),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Species = speciesSummary
                };
                var sightingSummaryHabitat = new SightingSummaryHabitat()
                {
                    Id = new MongoDBRef("Sightings", sightingsId),
                    Location = s.Location,
                    SightingTime = s.SightingTime,
                    Sighter = userSummary,
                    Species = speciesSummary
                };
                var sightingSummarySpecies = new SightingSummarySpecies()
                {
                    Id = new MongoDBRef("Sightings", sightingsId),
                    Location = s.Location,
                    Sighter = userSummary,
                    SightingTime = s.SightingTime,
                };

                var userSightingsFilter = Builders<User>.Filter.ElemMatch(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", new ObjectId(sightingsId))));
                var userSightingsUpdate = Builders<User>.Update.Set("Sightings.$", sightingSummaryUser);

                var habitatSightingsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", new ObjectId(sightingsId))));
                var habitatSightingsUpdate = Builders<Habitat>.Update.Set("Sightings.$", sightingSummaryHabitat);

                var speciesSightingsFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Id == new MongoDBRef("Sightings", new ObjectId(sightingsId))));
                var speciesSightingsUpdate = Builders<Species>.Update.Set("Sightings.$", sightingSummarySpecies);


                await _context.Users.UpdateManyAsync(session, userSightingsFilter, userSightingsUpdate);
                await _context.Habitats.UpdateManyAsync(session, habitatSightingsFilter, habitatSightingsUpdate);
                await _context.Species.UpdateManyAsync(session, speciesSightingsFilter, speciesSightingsUpdate);

                var sighting = await _context.Sightings.Find(sig => sig.Id == sightingsId).FirstOrDefaultAsync();
                await session.CommitTransactionAsync();
                return sighting;
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Sighting service failed");
            }
        }
        //public async Task<Sighting> UpdateAsync(string sightingsId, SightingDto s)
        //{
        //    await DeleteAsync(sightingsId);
        //    await CreateAsync(s);
        //    return await _context.Sightings.Find(s=>s.Id==sightingsId).FirstOrDefaultAsync();
        //    //using var session = await _context.MongoClient.StartSessionAsync();
        //    //session.StartTransaction();
        //    //try
        //    //{
        //    //    var species = await _context.Species.Find(sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
        //    //    var speciesId = species.Id ?? "";
        //    //    if (species == default)
        //    //    {
        //    //        var newSpecies = new SpeciesDto()
        //    //        {
        //    //            Class = s.SpeciesClass,
        //    //            CommonName = s.CommonName,
        //    //            ScientificName = s.ScientificName,
        //    //            ImageUrl = s.PhotoUrl ?? "",
        //    //            Description = s.Description,
        //    //            ConsertvationStatus = s.ConservationStatus
        //    //        };

        //    //        speciesId = await _speciesService.CreateAsync(newSpecies);
        //    //        species = await _context.Species.Find(sp => sp.CommonName == s.CommonName && sp.ScientificName == s.ScientificName).FirstOrDefaultAsync();
        //    //    }

        //    //    var speciesSummary = new SpeciesSummary()
        //    //    {
        //    //        Class = species.Class,
        //    //        CommonName = s.CommonName,
        //    //        ScientificName = s.ScientificName,
        //    //        ImageUrl = species.ImageUrl ?? "",
        //    //        Description = species.Description,
        //    //        ConservationStatus = species.ConservationStatus,
        //    //        Id = new MongoDBRef("Species", speciesId)
        //    //    };

        //    //    var user = await _context.Users.Find(u => u.Id == s.SighterId).FirstOrDefaultAsync();
        //    //    if (user == default)
        //    //    {
        //    //        throw new Exception("Invalid User Id");
        //    //    }

        //    //    var userSummary = new UserSummary()
        //    //    {
        //    //        Email = user.Email,
        //    //        Name = user.Name,
        //    //        NumberOfSightings = user.Sightings.Count() + 1,
        //    //        Id = new MongoDBRef("Users", user.Id)
        //    //    };

        //    //    var sighting = new Sighting()
        //    //    {
        //    //        SightingTime = s.SightingTime,
        //    //        Location = s.Location,
        //    //        ImageUrl = s.PhotoUrl ?? "",
        //    //        Sighter = userSummary,
        //    //        Species = speciesSummary
        //    //    };
        //    //    await _context.Sightings.ReplaceOneAsync(sightingsId,sighting);


        //    //    var sightingSummaryUser = new SightingSummaryUser()
        //    //    {
        //    //        Id = new MongoDBRef("Sightings", sighting.Id),
        //    //        Location = s.Location,
        //    //        SightingTime = s.SightingTime,
        //    //        Species = speciesSummary
        //    //    };
        //    //    //var userFilter = Builders<User>.Filter.Where(u => u.Id == user.Id);
        //    //    //var userUpdate = Builders<User>.Update.Push(u => u.Sightings, sightingSummaryUser);
        //    //    var userSightingsFilter = Builders<User>.Filter.ElemMatch(u => u.Sightings, Builders<SightingSummaryUser>.Filter.Where(s => s.Id == new MongoDBRef("Sightings", sightingsId)));
        //    //    var userSightingsUpdate = Builders<User>.Update.Set("Sightings.$", sightingSummaryUser);
        //    //    await _context.Users.UpdateOneAsync(userSightingsFilter, userSightingsUpdate);


        //    //    var sightingSummarySpecies = new SightingSummarySpecies()
        //    //    {
        //    //        Id = new MongoDBRef("Sightings", sighting.Id),
        //    //        Location = s.Location,
        //    //        SightingTime = s.SightingTime,
        //    //        Sighter = userSummary
        //    //    };
        //    //    //var speciesFilter = Builders<Species>.Filter.Where(sp => sp.Id == speciesId);
        //    //    //var speciesUpdate = Builders<Species>.Update.Push(sp => sp.Sightings, sightingSummarySpecies);
        //    //    var speciesSightingsFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Id == new MongoDBRef("Sightings", sightingsId)));
        //    //    var speciesSightingsUpdate = Builders<Species>.Update.Set("Sightings.$", sightingSummarySpecies);
        //    //    await _context.Species.UpdateOneAsync(speciesSightingsFilter, speciesSightingsUpdate);

        //    //    HabitatDto habitatDto = new HabitatDto()
        //    //    {
        //    //        BorderPoints = new List<GeoJsonPoint<GeoJson2DGeographicCoordinates>>(),
        //    //        Sightings = new List<SightingSummaryHabitat>()
        //    //    };
        //    //    var point = GeoJson.Point(new GeoJson2DGeographicCoordinates(s.Location.Longitude, s.Location.Latitude));
        //    //    habitatDto.BorderPoints.Add(point);
        //    //    var sightingSummaryHabitat = new SightingSummaryHabitat()
        //    //    {
        //    //        Id = new MongoDBRef("Sightings", sighting.Id),
        //    //        Location = s.Location,
        //    //        SightingTime = s.SightingTime,
        //    //        Sighter = userSummary,
        //    //        Species = speciesSummary
        //    //    };
        //    //    habitatDto.Sightings.Add(sightingSummaryHabitat);
        //    //    await _habitatService.UpdateAsync()

        //    //    return sighting.Id;
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    await session.AbortTransactionAsync();
        //    //    throw new Exception("Sighting failed");
        //    //}
        //}


    }
}

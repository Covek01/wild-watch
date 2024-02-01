using Amazon.Runtime.Internal.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Drawing;
using WildWatchAPI.Context;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;

namespace WildWatchAPI.Services
{
    public class HabitatService : IHabitatService
    {
        private readonly IDbContext _context;
        private readonly IConfiguration _configuration;

        public HabitatService(
            IDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            ////var indexKeysDefinition = Builders<Habitat>.IndexKeys.Geo2DSphere(h => h.BorderPoints);
            ////var indexModel = new CreateIndexModel<Habitat>(indexKeysDefinition);
            ////  _context.Habitats.Indexes.CreateOneAsync(indexModel);
            //var indexKeysDefinition = Builders<Habitat>.IndexKeys.Geo2DSphere(h => h.BorderPoints);
            //var indexModel = new CreateIndexModel<Habitat>(indexKeysDefinition);

            //// Create the index only if it doesn't exist
            //var indexCursor = _context.Habitats.Indexes.List();
            //var indexes =  indexCursor.ToList();
            //if (indexes.All(index => index["name"].AsString != indexModel.Options.Name))
            //{
            //    _context.Habitats.Indexes.CreateOne(indexModel);
            //}


            //var keys = Builders<Habitat>.IndexKeys.Geo2DSphere("BorderPoints");
            //var model = new CreateIndexModel<Habitat>(keys);
            //_context.Habitats.Indexes.CreateOne(model);
            var indexDef = Builders<Habitat>.IndexKeys.Geo2DSphere("BorderPoints.Location");
            var indexModel = new CreateIndexModel<Habitat>(indexDef);
            _context.Habitats.Indexes.CreateOne(indexModel);
        }

        private bool CloserThanMeters(GeoJsonPoint<GeoJson2DGeographicCoordinates> p, List<GeoJsonPoint<GeoJson2DGeographicCoordinates>> pointsList)
        {
            var maxDistance = double.Parse(_configuration.GetSection("MaxDistanceHabitatMeters").Value??"0");
            foreach (var point in pointsList)
            {
                var distance = Math.Sqrt(Math.Pow((point.Coordinates.Latitude - p.Coordinates.Latitude), 2) + Math.Pow((point.Coordinates.Longitude - p.Coordinates.Longitude), 2))*111111;

                if (distance < maxDistance) return true;
            }
            return false;
        }
        
        private double CalculateDistance(GeoJsonPoint<GeoJson2DGeographicCoordinates> p1, GeoJsonPoint<GeoJson2DGeographicCoordinates> p2)
        {
            return Math.Sqrt(Math.Pow((p1.Coordinates.Latitude - p1.Coordinates.Latitude), 2) + Math.Pow((p2.Coordinates.Longitude - p2.Coordinates.Longitude), 2)) * 111111;
        }
        public async Task ConsolidateAsync(string habitatId)
        {
            var habitat = await _context.Habitats.Find(h => h.Id == habitatId).FirstOrDefaultAsync();
            if (habitat == default)
            {
                throw new Exception("Invalid Id");
            }
            var maxDistance = double.Parse(_configuration.GetSection("MaxDistanceHabitatMeters").Value ?? "0");

            using var session = await _context.MongoClient.StartSessionAsync();
            session.StartTransaction();
            try
            {
                int numOfConsolidation = 0;
                do
                {
                    numOfConsolidation = 0;

                    //var nearHabitats = await _context.Habitats.Find(h => h.BorderPoints.Any(p => CloserThanMeters(p, habitat.BorderPoints))).ToListAsync();
                    //var nearHabitatsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.BorderPoints, Builders<GeoJsonPoint<GeoJson2DGeographicCoordinates>>.Filter.Where(p => CloserThanMeters(p, habitat.BorderPoints)));
                    var nearHabitatsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.BorderPoints, Builders<GeoJsonPoint<GeoJson2DGeographicCoordinates>>.Filter.Where(p => p.Coordinates.Longitude == 45));

                    var nearHabitats = await _context.Habitats.Find(session,nearHabitatsFilter).ToListAsync();





                    numOfConsolidation = nearHabitats.Count;

                    nearHabitats.ForEach(async h => {
                        habitat.Sightings.AddRange(h.Sightings);
                        habitat.BorderPoints.AddRange(h.BorderPoints);
                        await DeleteAsync(h.Id);
                    });

                } while (numOfConsolidation > 0);

                await UpdateAsync(habitatId,
                    new HabitatDto()
                    {
                        BorderPoints=habitat.BorderPoints,
                        Sightings=habitat.Sightings
                    }
                 );

                await session.CommitTransactionAsync();
            }
            catch(Exception ) 
            {
                await session.AbortTransactionAsync();
            }
        }
        public async Task<string> CreateAsync(HabitatDto h)
        {
            //using var session = await _context.MongoClient.StartSessionAsync();
            //session.StartTransaction();
            //try
            //{
                Habitat habitat = new Habitat()
                {
                    BorderPoints = new List<GeoJsonPoint<GeoJson2DGeographicCoordinates>>(),
                    Sightings = new List<SightingSummaryHabitat>()
                };
                //habitat.BorderPoints.Add(GeoJson.Point(new GeoJson2DGeographicCoordinates(20, 20)));
               
                habitat.BorderPoints.Add(new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(20, 30)));
                await _context.Habitats.InsertOneAsync( habitat);
                //var borderPoint = h.BorderPoints[0];
                //Console.WriteLine($"BorderPoint: Type={borderPoint.Type}, Coordinates=[{borderPoint.Coordinates.Longitude}, {borderPoint.Coordinates.Latitude}]");

                //habitat.BorderPoints.Add(borderPoint);
                //habitat.Sightings.Add(h.Sightings[0]);
                //await _context.Habitats.InsertOneAsync(session, habitat);
                ////habitat.BorderPoints.Add(h.BorderPoints[0]);
                ////habitat.Sightings.Add(h.Sightings[0]);
                ////await _context.Habitats.InsertOneAsync(session, habitat);

                //////////////////////////////////////////HabitatSummary habitatSummary = new HabitatSummary()
                //////////////////////////////////////////{
                //////////////////////////////////////////    BorderPoints = h.BorderPoints,
                //////////////////////////////////////////    Id = new MongoDBRef("Habitats",habitat.Id)
                //////////////////////////////////////////};

                //////////////////////////////////////////var speciesToUpdate = h.Sightings.Select(s => s.Species.Id.Id.ToString()).ToList();
                //////////////////////////////////////////var speciesFilter = Builders<Species>.Filter.In(s => s.Id, speciesToUpdate);
                //////////////////////////////////////////var speciesUpdate = Builders<Species>.Update.Push(s => s.Habitats, habitatSummary);
                //////////////////////////////////////////await _context.Species.UpdateManyAsync( speciesFilter, speciesUpdate);

                //////////////////////////////////////////var sourceHabitat = await _context.Habitats.Find(h => h.Id == habitat.Id).FirstOrDefaultAsync();

                //throw new Exception("SIKE");
                //await session.CommitTransactionAsync();
                return "not done";
            //}
            //catch (Exception)
            //{
                //await session.AbortTransactionAsync();
                //throw new Exception("Habitat service failed");
            //}
        }
        //public async Task<string> CreateAsync(HabitatDto h)
        //{
        //    using var session = await _context.MongoClient.StartSessionAsync();
        //    session.StartTransaction();
        //    try
        //    {
        //        Habitat habitat = new Habitat()
        //        {
        //            BorderPoints = h.BorderPoints,
        //            Sightings = h.Sightings
        //        };
        //        await _context.Habitats.InsertOneAsync(session, habitat);

        //        HabitatSummary habitatSummary = new HabitatSummary()
        //        {
        //            BorderPoints = h.BorderPoints,
        //            Id = new MongoDBRef("Habitats", habitat.Id)
        //        };

        //        var speciesToUpdate = h.Sightings.Select(s => s.Species.Id.Id.ToString()).ToList();
        //        var speciesFilter = Builders<Species>.Filter.In(s => s.Id, speciesToUpdate);
        //        var speciesUpdate = Builders<Species>.Update.Push(s => s.Habitats, habitatSummary);
        //        await _context.Species.UpdateManyAsync(session, speciesFilter, speciesUpdate);

        //        await ConsolidateAsync(habitat.Id);

        //        await session.CommitTransactionAsync();
        //        return habitat.Id;
        //    }
        //    catch(Exception)
        //    {
        //        await session.AbortTransactionAsync();
        //        throw new Exception("Habitat failed");
        //    }
        //}

        public async Task DeleteAsync(string habitatId)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var habitat = Builders<Habitat>.Filter.Where(h => h.Id == habitatId);

                var speciesFilter = Builders<Species>.Filter.ElemMatch(s => s.Habitats, Builders<HabitatSummary>.Filter.Where(h => h.Id == new MongoDBRef("Habitats", habitatId)));
                var speciesUpdate = Builders<Species>.Update.PullFilter(s => s.Habitats, Builders<HabitatSummary>.Filter.Where(h => h.Id == new MongoDBRef("Habitats", habitatId)));

                await _context.Habitats.DeleteOneAsync(session, habitat);
                await _context.Species.UpdateManyAsync(session, speciesFilter, speciesUpdate);

                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
            }
        }

        public async Task<Habitat> GetAsync(string habitatId)
        {
            var habitat= await _context.Habitats.Find(s => s.Id == habitatId).FirstOrDefaultAsync();
            if (habitat== default)
            {
                throw new Exception("Invalid Id");
            }
            return habitat;
        }

        public async Task<List<Habitat>> GetWithNumberOfSightings(int numberOfSightings)
        {
            var habitats = await _context.Habitats.Find(s => s.Sightings.Count >= numberOfSightings).ToListAsync();

            return habitats;
        }

        public async Task<Habitat> UpdateAsync(string habitatId, HabitatDto h)
        {
            using var session =await _context.MongoClient.StartSessionAsync();
            try
            {
                session.StartTransaction();

                var oldHabitat = await _context.Habitats.Find(h => h.Id == habitatId).FirstOrDefaultAsync();
                Habitat habitat = new Habitat()
                {
                    BorderPoints = h.BorderPoints,
                    Sightings = h.Sightings,
                    Id = habitatId
                };
                await _context.Habitats.ReplaceOneAsync(habitatId, habitat);

                HabitatSummary habitatSummary = new HabitatSummary()
                {
                    Id = new MongoDBRef("Habitats", habitatId),
                    BorderPoints = h.BorderPoints
                };

                var speciesToRemoveUpdate = oldHabitat.Sightings.Select(s => s.Species.Id.Id.ToString()).ToList();
                //var speciesRemoveFilter = Builders<Species>.Filter.ElemMatch(s => s.Habitats, Builders<HabitatSummary>.Filter.Where(h => h.Id == new MongoDBRef("Habitats", habitatId)));
                var speciesRemoveFilter = Builders<Species>.Filter.In(s => s.Id, speciesToRemoveUpdate);
                var speciesRemoveUpdate = Builders<Species>.Update.PullFilter(s => s.Habitats, Builders<HabitatSummary>.Filter.Where(h => h.Id == new MongoDBRef("Habitats", habitatId)));
                await _context.Species.UpdateManyAsync(speciesRemoveFilter, speciesRemoveUpdate);

                var speciesToAddUpdate = h.Sightings.Select(s => s.Species.Id.Id.ToString()).ToList();
                var speciesAddFilter = Builders<Species>.Filter.In(s => s.Id, speciesToAddUpdate);
                var speciesAddUpdate = Builders<Species>.Update.Push(s => s.Habitats, habitatSummary);
                await _context.Species.UpdateManyAsync(speciesAddFilter, speciesAddUpdate);

                //var speciesFilter = Builders<Species>.Filter.ElemMatch(s => s.Habitats, Builders<HabitatSummary>.Filter.Where(h => h.Id == new MongoDBRef("Habitats", habitatId)));
                //var speciesUpdate = Builders<Species>.Update.Set("Habitats.$", habitatSummary);
                //await _context.Species.UpdateManyAsync(speciesFilter, speciesUpdate);
                await session.CommitTransactionAsync();

                return habitat;
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("Habitat service failed");
            }
        }
    }
}

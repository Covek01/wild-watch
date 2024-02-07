using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Net.Mail;
using System.Net;
using WildWatchAPI.Context;
using WildWatchAPI.DTOs;
using WildWatchAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace WildWatchAPI.Services
{
    public class UserService:IUserService
    {
        public readonly IDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IDbContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string ExtendToken()
        {
            var id = string.Empty;
            var email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                throw new Exception("Token error");
            }



            if (string.IsNullOrEmpty(id)||string.IsNullOrEmpty(email))
            {
                throw new Exception("Token error");
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,id),
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }

        public async Task<UserDto> LogIn([FromBody] LogInUserDto user)
        {
            var userFromDatabase = await _context.Users.Find(u => u.Email == user.Email&& u.AccountConfirmed==true).FirstOrDefaultAsync();
            if(userFromDatabase == null)
            {
                throw new Exception("Username or Password wrong.");
            }

            if(!BCrypt.Net.BCrypt.Verify(user.Password,userFromDatabase.PasswordHash))
            {
                throw new Exception("Username or Password wrong");
            }

            string token = CreateToken(userFromDatabase);
            UserDto returnUser = new UserDto(userFromDatabase, token);
            return returnUser;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        public async Task<User> Registration([FromBody] RegistrationUserDto user)
        {
            var userExists = await _context.Users.Find(u => u.Email == user.Email && u.AccountConfirmed==true).FirstOrDefaultAsync();
            if (user== default)
            {
                throw new Exception("Mail taken");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            byte[] tokenBytes=Guid.NewGuid().ToByteArray();
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
            var nameStrings = user.Name.Split(" ");
            string generatedImage = $"https://ui-avatars.com/api/?background=f18d00&color=fff&name={nameStrings.First()}+{nameStrings.Last()}&rounded=true";
            var newUser = new User()
            {
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? generatedImage : user.ImageUrl,
                Name = user.Name,
                PasswordHash = passwordHash,
                VeficitationCode = codeEncoded,
                Location=null,
                AccountConfirmed= false,
            };

            await _context.Users.InsertOneAsync(newUser);

            var confirmationLink = $"https://localhost:7139/user/verifyMail?email={user.Email}&code={codeEncoded}";

            String poruka;
            poruka = $"Welcome {user.Name},\n\nPlease confirm your account registered on WildWatch with this email adress on link down below.\n" +
                confirmationLink + "\n\nWelcome to WildWatch!";

            var emailAdress = _configuration.GetSection("MailCredentials:email").Value!.ToString();
            var password = _configuration.GetSection("MailCredentials:password").Value!.ToString();

            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = emailAdress,
                    Password = password
                }
            };

            MailAddress fromMail = new MailAddress(emailAdress, "WildWatch");
            MailAddress toMail = new MailAddress(user.Email, user.Name);
            MailMessage message = new MailMessage()
            {
                From = fromMail,
                Subject = "Confirm your WildWatch account",
                Body = poruka
            };

            message.To.Add(toMail);
            await Client.SendMailAsync(message);


            return newUser;

        }

        public async Task VerifyUser(string email, string verificationCode)
        {
            var user= await _context.Users.Find(u => u.Email == email && u.AccountConfirmed == false).FirstOrDefaultAsync();

            if(user == null)
            {
                throw new Exception("Verification fail");
            }

            var filter = Builders<User>.Filter.Where(u => u.Email == email);
            var update = Builders<User>.Update
                .Set(u => u.AccountConfirmed, true);

            await _context.Users.UpdateOneAsync(filter, update);

        }

        public async Task DeleteAsync(string userId)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var user = Builders<User>.Filter.Where(u => u.Id == userId);

                var sightings = Builders<Sighting>.Filter.Where(s => s.Sighter.userId == userId);

                var speciesFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Sighter.userId ==  userId));
                var speciesUpdate = Builders<Species>.Update.PullFilter(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Sighter.userId == userId));

                var habitatsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.userId == userId));
                var habitatsUpdate = Builders<Habitat>.Update.PullFilter(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.userId == userId));


                await _context.Users.DeleteOneAsync(session,user);
                await _context.Sightings.DeleteManyAsync(session, sightings);
                await _context.Species.UpdateManyAsync(session, speciesFilter, speciesUpdate);
                await _context.Habitats.UpdateManyAsync(session, habitatsFilter, habitatsUpdate);
                await session.CommitTransactionAsync();
            }
            catch(Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("User service failed");
            }
        }

        public async Task<User> GetAsync(string userId)
        {
            var user =await _context.Users.Find(u=>u.Id==userId).FirstOrDefaultAsync();
            if (user == default)
            {
                throw new Exception("Invalid Id");
            }
            return user;
        }

        //public async Task<User> UpdateAsync(string userId, UserUpdateDto user)
        //{
        //    using var session = await _context.MongoClient.StartSessionAsync();
        //    session.StartTransaction();
        //    try
        //    {
        //        var userFilter = Builders<User>.Filter.Where(u => u.Id == userId);
        //        var userUpdate = Builders<User>.Update
        //            .Set(u => u.Name, user.Name)
        //            .Set(u => u.Email, user.Email)
        //            .Set(u => u.AccountConfirmed, user.AccountConfirmed)
        //            .Set(u => u.ImageUrl, user.ImageUrl)
        //            .Set(u => u.DateOfBirth, user.DateOfBirth)
        //            .Set(u => u.Location, user.Location);

        //        await _context.Users.UpdateOneAsync(session, userFilter, userUpdate);

        //        var userFromDatabase = await _context.Users.Find(session, userFilter).FirstOrDefaultAsync();
        //        if (userFromDatabase == default)
        //        {
        //            throw new Exception("How???");
        //        }

        //        var userSummary = new UserSummary()
        //        {
        //            Id = new MongoDBRef("Users", userId),
        //            Email = user.Email,
        //            Name = user.Name,
        //            NumberOfSightings = userFromDatabase.Sightings.Count()
        //        };

        //        //var speciesFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Sighter.Id == new MongoDBRef("Users", userId)));
        //        ////var speciesUpdate = Builders<Species>.Update.PullFilter(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Sighter.Id == new MongoDBRef("Users", userId)));
        //        //var speciesUpdate = Builders<Species>.Update.Set("Sightings.Sighter.$", userSummary);
        //        //await _context.Species.UpdateManyAsync(session,speciesFilter, speciesUpdate);

        //        //var habitatsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.Id == new MongoDBRef("Users", userId)));
        //        ////var habitatsUpdate = Builders<Habitat>.Update.PullFilter(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.Id == new MongoDBRef("Users", userId)));
        //        //var habitatsUpdate= Builders<Habitat>.Update.Set("Sightings.Sighter.$", userSummary);
        //        //await _context.Habitats.UpdateManyAsync(session, habitatsFilter, habitatsUpdate);

        //        //await session.CommitTransactionAsync();
        //        //return userFromDatabase;
        //        var speciesFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Sighter.Id == new MongoDBRef("Users", userId)));
        //        var speciesUpdate = Builders<Species>.Update.Set("Sightings.$[element].Sighter", userSummary);
        //        var speciesUpdateOptions = new UpdateOptions { ArrayFilters = new List<ArrayFilterDefinition> { new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("element.Sighter.Id.Id", userId)) } };
        //        await _context.Species.UpdateManyAsync(session, speciesFilter, speciesUpdate, speciesUpdateOptions);


        //        var habitatsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.Id == new MongoDBRef("Users", userId)));
        //        var habitatsUpdate = Builders<Habitat>.Update.Set("Sightings.$[element].Sighter", userSummary);
        //        var habitatsUpdateOptions = new UpdateOptions { ArrayFilters = new List<ArrayFilterDefinition> { new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("element.Sighter.Id.Id", userId)) } };
        //        await _context.Habitats.UpdateManyAsync(session, habitatsFilter, habitatsUpdate, habitatsUpdateOptions);


        //        await session.CommitTransactionAsync();
        //        return userFromDatabase;
        //    }
        //    catch (Exception)
        //    {
        //        await session.AbortTransactionAsync();
        //        throw new Exception("User service failed");
        //    }
        //}

        public async Task<User> UpdateAsync(string userId, UserUpdateDto user)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var userFilter = Builders<User>.Filter.Where(u => u.Id == userId);
                var userUpdate = Builders<User>.Update
                    .Set(u => u.Name, user.Name)
                    .Set(u => u.Email, user.Email)
                    .Set(u => u.AccountConfirmed, user.AccountConfirmed)
                    .Set(u => u.ImageUrl, user.ImageUrl)
                    .Set(u => u.DateOfBirth, user.DateOfBirth)
                    .Set(u => u.Location, user.Location);

                await _context.Users.UpdateOneAsync(session, userFilter, userUpdate);

                var userFromDatabase = await _context.Users.Find(session, userFilter).FirstOrDefaultAsync();
                if (userFromDatabase == default)
                {
                    throw new Exception("How???");
                }

                var userSummary = new UserSummary()
                {
                    userId = userId,
                    Email = user.Email,
                    Name = user.Name,
                    NumberOfSightings = userFromDatabase.Sightings.Count()
                };

                var sightingsFilter = Builders<Sighting>.Filter.Where(s => s.Sighter.userId == userId);
                var sightingsUpdate = Builders<Sighting>.Update.Set(sig => sig.Sighter, userSummary);
                await _context.Sightings.UpdateManyAsync(session, sightingsFilter,sightingsUpdate);

                var speciesFilter = Builders<Species>.Filter.ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(si => si.Sighter.userId == userId));
                //var speciesFilter = Builders<Species>.Filter.Empty;
                var speciesUpdate = Builders<Species>.Update.Set("Sightings.$[s].Sighter", userSummary);
                var speciesArrayFilters = new List<ArrayFilterDefinition>
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("s.Sighter.userId",userId))
                };
                //var speciesArrayFilters = new List<ArrayFilterDefinition>
                //{
                //    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                //       new BsonDocument("s.Sighter.$id", new BsonDocument("$eq",userId)))
                //};
                var speciesUpdateOptions = new UpdateOptions { ArrayFilters = speciesArrayFilters };
                await _context.Species.UpdateManyAsync(session, speciesFilter, speciesUpdate,speciesUpdateOptions);


                var habitatsFilter = Builders<Habitat>.Filter.ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.userId == userId));
                //var habitatsFilter = Builders<Habitat>.Filter.Empty;
                var habitatsUpdate = Builders<Habitat>.Update.Set("Sightings.$[s].Sighter", userSummary);
                var habitatArrayFilters = new List<ArrayFilterDefinition>
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("s.Sighter.userId",userId))
                };

                var habitatUpdateOptions = new UpdateOptions { ArrayFilters = habitatArrayFilters };
                await _context.Habitats.UpdateManyAsync(session, habitatsFilter, habitatsUpdate,habitatUpdateOptions);

                await session.CommitTransactionAsync();
                return userFromDatabase;
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("User service failed");
            }
        }

        public async Task<User> AddMyFavouriteSpecies(string speciesId)
        {
            var id = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Token error");
                }
            }
            else
            {
                throw new Exception("Token error");
            }
            var userFilter = Builders<User>.Filter.Where(u => u.Id == id&& !u.FavouriteSpecies.Contains(new MongoDBRef("Species", speciesId)));
            var userUpdate = Builders<User>.Update.Push(u => u.FavouriteSpecies, new MongoDBRef("Species", speciesId));
            await _context.Users.UpdateOneAsync(userFilter, userUpdate);

            return await _context.Users.Find(userFilter).FirstOrDefaultAsync();

        }

        public async Task<User> RemoveMyFavouriteSpecies(string speciesId)
        {
            var id = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Token error");
                }
            }
            else
            {
                throw new Exception("Token error");
            }
            var userFilter = Builders<User>.Filter.Where(u => u.Id == id);
            var userUpdate = Builders<User>.Update.Pull(u => u.FavouriteSpecies, new MongoDBRef("Species", speciesId));
            await _context.Users.UpdateOneAsync(userFilter, userUpdate);

            return await _context.Users.Find(userFilter).FirstOrDefaultAsync();

        }

        public async Task SetMyLocation(GeoJson2DGeographicCoordinates? location)
        {
            var id = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Token error");
                }
            }
            else
            {
                throw new Exception("Token error");
            }
            var userFilter = Builders<User>.Filter.Where(u => u.Id == id);
            var userUpdate = Builders<User>.Update.Set(u => u.Location, location);

            await _context.Users.UpdateOneAsync(userFilter, userUpdate);
        }

        public async Task<GeoJson2DGeographicCoordinates?> GetMyLocation()
        {
            var id = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Token error");
                }
            }
            else
            {
                throw new Exception("Token error");
            }
            var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("Invalid Id");
            }
            return user.Location;
        }

        public async Task SetUserName(string id, string userName)
        {
            using var session = await _context.MongoClient.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var user = Builders<User>.Filter.Where(u => u.Id == id);
                var update = Builders<User>.Update
                    .Set(u => u.Name, userName);

                var userSightings = Builders<Sighting>.Filter.Where(u => u.Sighter.userId == id);
                var userSightingsUpdate = Builders<Sighting>.Update
                    .Set(s => s.Sighter.Name, userName);


                var userHabitat = Builders<Habitat>.Filter
                    .ElemMatch(h => h.Sightings, Builders<SightingSummaryHabitat>.Filter.Where(s => s.Sighter.userId == id));
                var userHabitatUpdate = Builders<Habitat>.Update
                    .Set("Sightings.$[s].Sighter.Name", userName);
                var habitatArrayFilters = new List<ArrayFilterDefinition>
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("s.Sighter.userId", id))
                };
                var habitatUpdateOptions = new UpdateOptions { ArrayFilters = habitatArrayFilters };

                var userSpecies = Builders<Species>.Filter
                   .ElemMatch(s => s.Sightings, Builders<SightingSummarySpecies>.Filter.Where(s => s.Sighter.userId == id));
                var userSpeciesUpdate = Builders<Species>.Update
                    .Set("Sightings.$[s].Sighter.Name", userName);
                var speciesArrayFilters = new List<ArrayFilterDefinition>
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("s.Sighter.userId", id))
                };
                var speciesUpdateOptions = new UpdateOptions { ArrayFilters = speciesArrayFilters };


                var result = await _context.Users.UpdateOneAsync(session, user, update);
                var resultSightings = await _context.Sightings.UpdateManyAsync(session, userSightings, userSightingsUpdate);
                var resultHabitat = await _context.Habitats.UpdateManyAsync(session, userHabitat, userHabitatUpdate, habitatUpdateOptions);
                var resultSpecies = await _context.Species.UpdateManyAsync(session, userSpecies, userSpeciesUpdate, speciesUpdateOptions);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw new Exception("User service failed");
            }
        }

        public async Task<List<SightingSummaryUser>> GetMySightings()
        {
            try
            {
                var id = string.Empty;
                if (_httpContextAccessor.HttpContext != null)
                {
                    id = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new Exception("Token error");
                    }
                }
                else
                {
                    throw new Exception("Token error");
                }

                var filter = Builders<User>.Filter.Where(u => u.Id == id);
                var user = await _context.Users.Find(filter).FirstOrDefaultAsync();

                return user.Sightings;
            }
            catch (Exception)
            {
                throw new Exception("User service failed");
            }
        }
    }
}

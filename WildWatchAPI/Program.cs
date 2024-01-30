using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WildWatchAPI.Context;
using WildWatchAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("Token").Value!))
    };
});

builder.Services.AddSingleton<IMongoClient>((settings) =>
{
    return new MongoClient(builder.Configuration.GetConnectionString("MongoDB"));
});
builder.Services.AddSingleton<IDbContext, DbContext>();
builder.Services.AddSingleton<ISpeciesService, SpeciesService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ISightingService, SightingService>();
builder.Services.AddSingleton<IHabitatService, HabitatService>();
builder.Services.AddHttpContextAccessor();



var DevelopmentOrigins = "CORSDevelopment";
var ProductionOrigins = "CORSProduction";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: DevelopmentOrigins, policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "https://localhost:3000",
            "http://127.0.0.1:3000",
            "https://127.0.0.1:3000"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    options.AddPolicy(name: ProductionOrigins, policy =>
    {
        policy
        .WithOrigins()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors(DevelopmentOrigins);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors(ProductionOrigins);
}

app.UseAuthorization();

app.MapControllers();

app.Run();

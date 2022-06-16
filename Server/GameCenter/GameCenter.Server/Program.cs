using GameCenter.Business.Behavior;
using GameCenter.Business.Filters;
using GameCenter.DataAccess.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using GameCenter.Business.Services.Genres;
using GameCenter.DataAccess.Repositories.Genres;
using GameCenter.Business.Helpers.FileStorage;
using GameCenter.Business.Services.Actors;
using GameCenter.DataAccess.Repositories.Actors;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using AutoMapper;
using GameCenter.Server.AutoMapper;
using GameCenter.DataAccess.Repositories.GameCenter;
using GameCenter.Business.Services.GameCenter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOption => 
    sqlOption.UseNetTopologySuite()));

// Add services to the container.
//builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers(options =>
{
    //options.Filters.Add(typeof(ExeptionFilter));
    options.Filters.Add(typeof(ParseBadRequest));
}).ConfigureApiBehaviorOptions(BadRequest.Parse);
AddTransient(builder.Services);
AddScoped(builder.Services);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton(provider => new MapperConfiguration(config =>
{
    var geometryFactory = provider.GetRequiredService<GeometryFactory>();
    config.AddProfile(new MappingProfiles(geometryFactory));
}).CreateMapper());

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices
     .Instance.CreateGeometryFactory(srid: 4326));
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    var frontendUrl = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
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
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddTransient(IServiceCollection services)
{
    services.AddTransient<IGenreService, GenreService>();
    services.AddTransient<IActorService, ActorService>();
    services.AddTransient<IGameCenterService, GameCenterService>();
}

static void AddScoped(IServiceCollection services)
{
    services.AddScoped<IFileStorageService, FileStorageService>();
    services.AddScoped<IGenreRepository, GenreRepository>();
    services.AddScoped<IActorRepository, ActorRepository>();
    services.AddScoped<IGameCenterRepository, GameCenterRepository>();
}

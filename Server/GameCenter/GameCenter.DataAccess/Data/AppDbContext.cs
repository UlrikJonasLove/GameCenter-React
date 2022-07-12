﻿
using GameCenter.Models.Actors;
using GameCenter.Models.Genres;
using GameCenter.Models.GameCenter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCenter.Models.Games;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GameCenter.Models.Rating;
using NetTopologySuite.Geometries;
using GameCenter.DataAccess.Seeders;

namespace GameCenter.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext([NotNull] DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GamesActors>()
                .HasKey(x => new { x.ActorId, x.GameId });

            builder.Entity<GamesGenres>()
                .HasKey(x => new { x.GenreId, x.GameId });

            builder.Entity<GameCentersGames>()
                .HasKey(x => new { x.GameCenterId, x.GameId });

            // IdentityDbContext is expecting to be called through this OnModelCreating
            base.OnModelCreating(builder);


            //GameSeeder game = new();
            //game.SeedGames(builder);

            //GenreSeeder genre = new();
            //genre.SeedGenres(builder);

            //ActorsSeeder actors = new();
            //actors.SeedActors(builder);
            
            //GamesGenresSeeder gamesGenres = new();
            //gamesGenres.SeedGamesGenres(builder);

            //GamesActorsSeeder gamesActors = new();
            //gamesActors.SeedGamesActors(builder);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<GameCenters> GameCenters { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamesActors> GamesActors { get; set; }
        public DbSet<GamesGenres> GamesGenres { get; set; }
        public DbSet<GameCentersGames> GameCentersGames { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}

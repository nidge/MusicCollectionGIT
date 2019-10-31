using System.Collections.Generic;
using MusicCollection2017.Models;

namespace MusicCollection2017.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MusicCollection2017.DAL.MusicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MusicCollection2017.DAL.MusicContext context)
        {
            var artists = new List<Artist>
            {
                new Artist {Id = 1, Title = "Oasis1"},
                new Artist {Id = 2, Title = "Blur1"},
                new Artist {Id = 3, Title = "Dylan, Bob1"}
            };

            artists.ForEach(s => context.Artists.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            var genres = new List<Genre>
            {
                new Genre {Id = 1, Title = "Indie"}
            };
            genres.ForEach(s => context.Genres.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            //var recordings = new List<Recording>
            //{
            //    new Recording {ArtistId = 1, Title = "Greatest Hits", Rating = Rating.A, InCloud=true, GenreId=1}
            //};

            //recordings.ForEach((s => context.Recordings.AddOrUpdate(p => p.ArtistId, s)));
            //context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

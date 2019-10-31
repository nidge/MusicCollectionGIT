using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicCollection2017.Models;

namespace MusicCollection2017.DAL
{
    public class MusicInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MusicContext>
    {
        protected override void Seed(MusicContext context)
        {
            var artists = new List<Artist>
            {
                new Artist { Id = 1, Title = "Oasis"},
                new Artist { Id = 2, Title = "Blur"}
            };

            artists.ForEach(s => context.Artists.Add(s));
            context.SaveChanges();

            var genres = new List<Genre>
            {
                new Genre { Id = 1, Title = "Indie"}
            };

            genres.ForEach(s => context.Genres.Add(s));
            context.SaveChanges();

            var recordings = new List<Recording>
            {
                new Recording { Id = 1, ArtistId = 1, GenreId = 1, Title = "Definitely Maybe"},
                new Recording { Id = 2, ArtistId = 1, GenreId = 1, Title = "Morning Glory"},
                new Recording { Id = 3, ArtistId = 2, GenreId = 1, Title = "Parklife"},
            };

            recordings.ForEach(s => context.Recordings.Add(s));
            context.SaveChanges();
        }
    }
}
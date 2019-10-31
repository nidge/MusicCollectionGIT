using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using MusicCollection2017.Models;

namespace MusicCollection2017.DAL
{
    public class MusicContext : DbContext
    {
        public MusicContext() : base("MusicContextConnectionString")
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Recording> Recordings { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // The table names in our DB will NOT be pluralized
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


    }
}
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GameStore.DAL.EF
{
    public class GameStoreContext : DbContext
    {

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }

        static GameStoreContext()
        {
            Database.SetInitializer(new GameStoreDbInitializer());
        }
        public GameStoreContext() : base("GameStore")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Comments)
                .WithRequired(c => c.Game)
                .WillCascadeOnDelete(true);

    

            modelBuilder.Entity<Game>().HasMany(c => c.Genres)
                .WithMany(s => s.Games)
                .Map(a => a.ToTable("GamesGenres"));

            modelBuilder.Entity<Game>().HasMany(c => c.PlatformTypes)
                .WithMany(s => s.Games)
                .Map(a => a.ToTable("GamesTypes"));





        }
    }
}

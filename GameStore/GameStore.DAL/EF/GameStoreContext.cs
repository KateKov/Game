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

        public GameStoreContext() : base("GameStore")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Comments)
                .WithRequired(c => c.Game)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Comment>()
                .HasOptional(c => c.ParentComment)
                .WithMany(c => c.ChildComments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .HasOptional(g => g.ParentGenre)
                .WithMany(a => a.ChildGenres)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Game>().HasMany(c => c.Genres)
                .WithMany(s => s.Games)
                .Map(t => t.MapLeftKey("GameId")
                    .MapRightKey("GenreId")
                    .ToTable("GamesGenres"));

            modelBuilder.Entity<Game>().HasMany(c => c.PlatformTypes)
                .WithMany(s => s.Games)
                .Map(t => t.MapLeftKey("GameId")
                    .MapRightKey("TypeId")
                    .ToTable("GamesTypes"));





        }
    }
}

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GameStore.DAL.Entities;
using GameStore.DAL.Entities.Translation;
using Order = GameStore.DAL.Entities.Order;
using OrderDetail = GameStore.DAL.Entities.OrderDetail;
using PlatformType = GameStore.DAL.Entities.PlatformType;
using Publisher = GameStore.DAL.Entities.Publisher;

namespace GameStore.DAL.EF
{
    public class GameStoreContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<GameTranslate> GameTranslates { get; set; }

        public DbSet<GenreTranslate> GenreTranslates { get; set; }

        public DbSet<PlatformTypeTranslate> PlatformTypeTranslates { get; set; }
        public DbSet<PublisherTranslate> PublisherTranslates { get; set; }
        public GameStoreContext(string connectionString) : base("GameStore") { }

        public GameStoreContext() { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Comments)
                .WithRequired(c => c.Game)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Genre>()
                .HasOptional(g => g.ParentGenre)
                .WithMany(a => a.ChildGenres)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Game>().HasMany(c => c.Genres)
                .WithMany(s => s.Games)
                .Map(t =>
                {
                    t.MapLeftKey("GameId");
                    t.MapRightKey("GenreId");
                    t.ToTable("GamesGenres");
                });

            modelBuilder.Entity<Game>().HasMany(c => c.PlatformTypes)
                .WithMany(s => s.Games)
                  .Map(t =>
                {
                    t.MapLeftKey("GameId");
                    t.MapRightKey("PlatformTypeId");
                    t.ToTable("GamesTypes");
                });
        }
    }
}

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GameStore.DAL.Entities;

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

        public GameStoreContext(string connectionString) : base("GameStore") { }
        static GameStoreContext()
        {
            Database.SetInitializer(new GameStoreDbInitializer());
        }

        public GameStoreContext() { }
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
                    .Map(a => a.ToTable("GamesGenres"));

            modelBuilder.Entity<Game>().HasMany(c => c.PlatformTypes)
                .WithMany(s => s.Games)
                    .Map(a => a.ToTable("GamesTypes"));
        }
    }
}

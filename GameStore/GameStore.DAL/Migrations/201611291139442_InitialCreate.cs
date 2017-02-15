namespace GameStore.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        GameId = c.Guid(nullable: false),
                        ParentCommentId = c.Guid(),
                        Quote = c.String(),
                        Id = c.String(),
                        ParentComment_EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Comment", t => t.ParentComment_EntityId)
                .Index(t => t.GameId)
                .Index(t => t.ParentComment_EntityId);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 65),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitsInStock = c.Short(nullable: false),
                        Discountinues = c.Boolean(nullable: false),
                        PublisherId = c.Guid(),
                        Viewing = c.Int(nullable: false),
                        DateOfAdding = c.DateTime(nullable: false),
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Publisher", t => t.PublisherId)
                .Index(t => t.Key, unique: true, name: "IX_key")
                .Index(t => t.PublisherId);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 65),
                        ParentId = c.Guid(),
                        Id = c.String(),
                        ParentGenre_EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Genre", t => t.ParentGenre_EntityId)
                .Index(t => t.Name, unique: true, name: "IY_name")
                .Index(t => t.ParentGenre_EntityId);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Short(nullable: false),
                        Discount = c.Single(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        GameId = c.Guid(nullable: false),
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsConfirmed = c.Boolean(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.EntityId);
            
            CreateTable(
                "dbo.PlatformType",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 65),
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.EntityId)
                .Index(t => t.Name, unique: true, name: "IZ_type");
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 65),
                        Description = c.String(),
                        HomePage = c.String(),
                        Id = c.String(),
                    })
                .PrimaryKey(t => t.EntityId)
                .Index(t => t.Name, unique: true, name: "IP_name");
            
            CreateTable(
                "dbo.GamesGenres",
                c => new
                    {
                        GameId = c.Guid(nullable: false),
                        GenreId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameId, t.GenreId })
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.Genre", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.GamesTypes",
                c => new
                    {
                        GameId = c.Guid(nullable: false),
                        PlatformTypeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.GameId, t.PlatformTypeId })
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.PlatformType", t => t.PlatformTypeId, cascadeDelete: true)
                .Index(t => t.GameId)
                .Index(t => t.PlatformTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "ParentComment_EntityId", "dbo.Comment");
            DropForeignKey("dbo.Game", "PublisherId", "dbo.Publisher");
            DropForeignKey("dbo.GamesTypes", "PlatformTypeId", "dbo.PlatformType");
            DropForeignKey("dbo.GamesTypes", "GameId", "dbo.Game");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.OrderDetail", "GameId", "dbo.Game");
            DropForeignKey("dbo.GamesGenres", "GenreId", "dbo.Genre");
            DropForeignKey("dbo.GamesGenres", "GameId", "dbo.Game");
            DropForeignKey("dbo.Genre", "ParentGenre_EntityId", "dbo.Genre");
            DropForeignKey("dbo.Comment", "GameId", "dbo.Game");
            DropIndex("dbo.GamesTypes", new[] { "PlatformTypeId" });
            DropIndex("dbo.GamesTypes", new[] { "GameId" });
            DropIndex("dbo.GamesGenres", new[] { "GenreId" });
            DropIndex("dbo.GamesGenres", new[] { "GameId" });
            DropIndex("dbo.Publisher", "IP_name");
            DropIndex("dbo.PlatformType", "IZ_type");
            DropIndex("dbo.OrderDetail", new[] { "GameId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Genre", new[] { "ParentGenre_EntityId" });
            DropIndex("dbo.Genre", "IY_name");
            DropIndex("dbo.Game", new[] { "PublisherId" });
            DropIndex("dbo.Game", "IX_key");
            DropIndex("dbo.Comment", new[] { "ParentComment_EntityId" });
            DropIndex("dbo.Comment", new[] { "GameId" });
            DropTable("dbo.GamesTypes");
            DropTable("dbo.GamesGenres");
            DropTable("dbo.Publisher");
            DropTable("dbo.PlatformType");
            DropTable("dbo.Order");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Genre");
            DropTable("dbo.Game");
            DropTable("dbo.Comment");
        }
    }
}

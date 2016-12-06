namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class localization : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Genre", "IY_name");
            DropIndex("dbo.PlatformType", "IZ_type");
            DropIndex("dbo.Publisher", "IP_name");
            CreateTable(
                "dbo.GenreTranslate",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 65),
                        Id = c.String(),
                        Genre_EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Genre", t => t.Genre_EntityId)
                .Index(t => t.Name, unique: true, name: "IY_name")
                .Index(t => t.Genre_EntityId);
            
            CreateTable(
                "dbo.PlatformTypeTranslate",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 65),
                        Id = c.String(),
                        PlatformType_EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.PlatformType", t => t.PlatformType_EntityId)
                .Index(t => t.Name, unique: true, name: "IZ_type")
                .Index(t => t.PlatformType_EntityId);
            
            CreateTable(
                "dbo.PublisherTranslate",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(maxLength: 65),
                        Id = c.String(),
                        Publisher_EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Publisher", t => t.Publisher_EntityId)
                .Index(t => t.Description, unique: true, name: "IP_name")
                .Index(t => t.Publisher_EntityId);
            
            CreateTable(
                "dbo.GameTranslate",
                c => new
                    {
                        EntityId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Id = c.String(),
                        Game_EntityId = c.Guid(),
                    })
                .PrimaryKey(t => t.EntityId)
                .ForeignKey("dbo.Game", t => t.Game_EntityId)
                .Index(t => t.Game_EntityId);
            
            DropColumn("dbo.Game", "Name");
            DropColumn("dbo.Game", "Description");
            DropColumn("dbo.Genre", "Name");
            DropColumn("dbo.PlatformType", "Name");
            DropColumn("dbo.Publisher", "Name");
            DropColumn("dbo.Publisher", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Publisher", "Description", c => c.String());
            AddColumn("dbo.Publisher", "Name", c => c.String(maxLength: 65));
            AddColumn("dbo.PlatformType", "Name", c => c.String(nullable: false, maxLength: 65));
            AddColumn("dbo.Genre", "Name", c => c.String(nullable: false, maxLength: 65));
            AddColumn("dbo.Game", "Description", c => c.String());
            AddColumn("dbo.Game", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.GameTranslate", "Game_EntityId", "dbo.Game");
            DropForeignKey("dbo.PublisherTranslate", "Publisher_EntityId", "dbo.Publisher");
            DropForeignKey("dbo.PlatformTypeTranslate", "PlatformType_EntityId", "dbo.PlatformType");
            DropForeignKey("dbo.GenreTranslate", "Genre_EntityId", "dbo.Genre");
            DropIndex("dbo.GameTranslate", new[] { "Game_EntityId" });
            DropIndex("dbo.PublisherTranslate", new[] { "Publisher_EntityId" });
            DropIndex("dbo.PublisherTranslate", "IP_name");
            DropIndex("dbo.PlatformTypeTranslate", new[] { "PlatformType_EntityId" });
            DropIndex("dbo.PlatformTypeTranslate", "IZ_type");
            DropIndex("dbo.GenreTranslate", new[] { "Genre_EntityId" });
            DropIndex("dbo.GenreTranslate", "IY_name");
            DropTable("dbo.GameTranslate");
            DropTable("dbo.PublisherTranslate");
            DropTable("dbo.PlatformTypeTranslate");
            DropTable("dbo.GenreTranslate");
            CreateIndex("dbo.Publisher", "Name", unique: true, name: "IP_name");
            CreateIndex("dbo.PlatformType", "Name", unique: true, name: "IZ_type");
            CreateIndex("dbo.Genre", "Name", unique: true, name: "IY_name");
        }
    }
}

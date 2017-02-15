namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeForeignKeyTransalte : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameTranslate", "BaseEntityId", "dbo.Game");
            DropForeignKey("dbo.GenreTranslate", "BaseEntityId", "dbo.Genre");
            DropForeignKey("dbo.PlatformTypeTranslate", "BaseEntityId", "dbo.PlatformType");
            DropForeignKey("dbo.PublisherTranslate", "BaseEntityId", "dbo.Publisher");
            DropIndex("dbo.GenreTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PlatformTypeTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PublisherTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.GameTranslate", new[] { "BaseEntityId" });
            AlterColumn("dbo.GenreTranslate", "BaseEntityId", c => c.Guid());
            AlterColumn("dbo.PlatformTypeTranslate", "BaseEntityId", c => c.Guid());
            AlterColumn("dbo.PublisherTranslate", "BaseEntityId", c => c.Guid());
            AlterColumn("dbo.GameTranslate", "BaseEntityId", c => c.Guid());
            CreateIndex("dbo.GenreTranslate", "BaseEntityId");
            CreateIndex("dbo.PlatformTypeTranslate", "BaseEntityId");
            CreateIndex("dbo.PublisherTranslate", "BaseEntityId");
            CreateIndex("dbo.GameTranslate", "BaseEntityId");
            AddForeignKey("dbo.GameTranslate", "BaseEntityId", "dbo.Game", "EntityId");
            AddForeignKey("dbo.GenreTranslate", "BaseEntityId", "dbo.Genre", "EntityId");
            AddForeignKey("dbo.PlatformTypeTranslate", "BaseEntityId", "dbo.PlatformType", "EntityId");
            AddForeignKey("dbo.PublisherTranslate", "BaseEntityId", "dbo.Publisher", "EntityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PublisherTranslate", "BaseEntityId", "dbo.Publisher");
            DropForeignKey("dbo.PlatformTypeTranslate", "BaseEntityId", "dbo.PlatformType");
            DropForeignKey("dbo.GenreTranslate", "BaseEntityId", "dbo.Genre");
            DropForeignKey("dbo.GameTranslate", "BaseEntityId", "dbo.Game");
            DropIndex("dbo.GameTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PublisherTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PlatformTypeTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.GenreTranslate", new[] { "BaseEntityId" });
            AlterColumn("dbo.GameTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PublisherTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PlatformTypeTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.GenreTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.GameTranslate", "BaseEntityId");
            CreateIndex("dbo.PublisherTranslate", "BaseEntityId");
            CreateIndex("dbo.PlatformTypeTranslate", "BaseEntityId");
            CreateIndex("dbo.GenreTranslate", "BaseEntityId");
            AddForeignKey("dbo.PublisherTranslate", "BaseEntityId", "dbo.Publisher", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.PlatformTypeTranslate", "BaseEntityId", "dbo.PlatformType", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.GenreTranslate", "BaseEntityId", "dbo.Genre", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.GameTranslate", "BaseEntityId", "dbo.Game", "EntityId", cascadeDelete: true);
        }
    }
}

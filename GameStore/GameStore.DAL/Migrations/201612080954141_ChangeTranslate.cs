namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTranslate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameTranslate", "Game_EntityId", "dbo.Game");
            DropForeignKey("dbo.GenreTranslate", "Genre_EntityId", "dbo.Genre");
            DropForeignKey("dbo.PlatformTypeTranslate", "PlatformType_EntityId", "dbo.PlatformType");
            DropForeignKey("dbo.PublisherTranslate", "Publisher_EntityId", "dbo.Publisher");
            DropIndex("dbo.GenreTranslate", new[] { "Genre_EntityId" });
            DropIndex("dbo.PlatformTypeTranslate", new[] { "PlatformType_EntityId" });
            DropIndex("dbo.PublisherTranslate", new[] { "Publisher_EntityId" });
            DropIndex("dbo.GameTranslate", new[] { "Game_EntityId" });
            DropColumn("dbo.GenreTranslate", "BaseEntityId");
            DropColumn("dbo.PlatformTypeTranslate", "BaseEntityId");
            DropColumn("dbo.PublisherTranslate", "BaseEntityId");
            DropColumn("dbo.GameTranslate", "BaseEntityId");
            RenameColumn(table: "dbo.GameTranslate", name: "Game_EntityId", newName: "BaseEntityId");
            RenameColumn(table: "dbo.GenreTranslate", name: "Genre_EntityId", newName: "BaseEntityId");
            RenameColumn(table: "dbo.PlatformTypeTranslate", name: "PlatformType_EntityId", newName: "BaseEntityId");
            RenameColumn(table: "dbo.PublisherTranslate", name: "Publisher_EntityId", newName: "BaseEntityId");
            AlterColumn("dbo.GenreTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PlatformTypeTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PublisherTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AlterColumn("dbo.GameTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.GenreTranslate", "BaseEntityId");
            CreateIndex("dbo.PlatformTypeTranslate", "BaseEntityId");
            CreateIndex("dbo.PublisherTranslate", "BaseEntityId");
            CreateIndex("dbo.GameTranslate", "BaseEntityId");
            AddForeignKey("dbo.GameTranslate", "BaseEntityId", "dbo.Game", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.GenreTranslate", "BaseEntityId", "dbo.Genre", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.PlatformTypeTranslate", "BaseEntityId", "dbo.PlatformType", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.PublisherTranslate", "BaseEntityId", "dbo.Publisher", "EntityId", cascadeDelete: true);
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
            AlterColumn("dbo.GameTranslate", "BaseEntityId", c => c.Guid());
            AlterColumn("dbo.PublisherTranslate", "BaseEntityId", c => c.Guid());
            AlterColumn("dbo.PlatformTypeTranslate", "BaseEntityId", c => c.Guid());
            AlterColumn("dbo.GenreTranslate", "BaseEntityId", c => c.Guid());
            RenameColumn(table: "dbo.PublisherTranslate", name: "BaseEntityId", newName: "Publisher_EntityId");
            RenameColumn(table: "dbo.PlatformTypeTranslate", name: "BaseEntityId", newName: "PlatformType_EntityId");
            RenameColumn(table: "dbo.GenreTranslate", name: "BaseEntityId", newName: "Genre_EntityId");
            RenameColumn(table: "dbo.GameTranslate", name: "BaseEntityId", newName: "Game_EntityId");
            AddColumn("dbo.GameTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.PublisherTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.PlatformTypeTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.GenreTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.GameTranslate", "Game_EntityId");
            CreateIndex("dbo.PublisherTranslate", "Publisher_EntityId");
            CreateIndex("dbo.PlatformTypeTranslate", "PlatformType_EntityId");
            CreateIndex("dbo.GenreTranslate", "Genre_EntityId");
            AddForeignKey("dbo.PublisherTranslate", "Publisher_EntityId", "dbo.Publisher", "EntityId");
            AddForeignKey("dbo.PlatformTypeTranslate", "PlatformType_EntityId", "dbo.PlatformType", "EntityId");
            AddForeignKey("dbo.GenreTranslate", "Genre_EntityId", "dbo.Genre", "EntityId");
            AddForeignKey("dbo.GameTranslate", "Game_EntityId", "dbo.Game", "EntityId");
        }
    }
}

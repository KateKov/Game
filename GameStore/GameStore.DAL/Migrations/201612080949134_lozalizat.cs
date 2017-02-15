namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lozalizat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GenreTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.PlatformTypeTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.PublisherTranslate", "BaseEntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.GameTranslate", "BaseEntityId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameTranslate", "BaseEntityId");
            DropColumn("dbo.PublisherTranslate", "BaseEntityId");
            DropColumn("dbo.PlatformTypeTranslate", "BaseEntityId");
            DropColumn("dbo.GenreTranslate", "BaseEntityId");
        }
    }
}

namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class localiz : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GenreTranslate", "Language", c => c.Int(nullable: false));
            AddColumn("dbo.PlatformTypeTranslate", "Language", c => c.Int(nullable: false));
            AddColumn("dbo.PublisherTranslate", "Language", c => c.Int(nullable: false));
            AddColumn("dbo.GameTranslate", "Language", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameTranslate", "Language");
            DropColumn("dbo.PublisherTranslate", "Language");
            DropColumn("dbo.PlatformTypeTranslate", "Language");
            DropColumn("dbo.GenreTranslate", "Language");
        }
    }
}

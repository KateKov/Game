namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GenreTranslate", "Name", c => c.String(maxLength: 65));
            AlterColumn("dbo.PublisherTranslate", "Name", c => c.String(nullable: false, maxLength: 65));
            CreateIndex("dbo.GenreTranslate", "Name", unique: true, name: "IY_name");
            CreateIndex("dbo.PlatformTypeTranslate", "Name", unique: true, name: "IZ_type");
            CreateIndex("dbo.PublisherTranslate", "Name", unique: true, name: "IP_name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PublisherTranslate", "IP_name");
            DropIndex("dbo.PlatformTypeTranslate", "IZ_type");
            DropIndex("dbo.GenreTranslate", "IY_name");
            AlterColumn("dbo.PublisherTranslate", "Name", c => c.String(maxLength: 65));
            AlterColumn("dbo.GenreTranslate", "Name", c => c.String(nullable: false, maxLength: 65));
        }
    }
}

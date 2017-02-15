namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changenameLocal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GenreTranslate", "IY_name");
            DropIndex("dbo.PlatformTypeTranslate", "IZ_type");
            DropIndex("dbo.PublisherTranslate", "IP_name");
            AlterColumn("dbo.PublisherTranslate", "Name", c => c.String(maxLength: 65));
            AlterColumn("dbo.PublisherTranslate", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PublisherTranslate", "Description", c => c.String(maxLength: 65));
            AlterColumn("dbo.PublisherTranslate", "Name", c => c.String());
            CreateIndex("dbo.PublisherTranslate", "Description", unique: true, name: "IP_name");
            CreateIndex("dbo.PlatformTypeTranslate", "Name", unique: true, name: "IZ_type");
            CreateIndex("dbo.GenreTranslate", "Name", unique: true, name: "IY_name");
        }
    }
}

namespace GameStore.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class profile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "ParentComment_EntityId", "dbo.Comment");
            DropForeignKey("dbo.Comment", "GameId", "dbo.Game");
            DropForeignKey("dbo.GamesGenres", "GameId", "dbo.Game");
            DropForeignKey("dbo.OrderDetail", "GameId", "dbo.Game");
            DropForeignKey("dbo.GamesTypes", "GameId", "dbo.Game");
            DropForeignKey("dbo.Genre", "ParentGenre_EntityId", "dbo.Genre");
            DropForeignKey("dbo.GamesGenres", "GenreId", "dbo.Genre");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.GamesTypes", "PlatformTypeId", "dbo.PlatformType");
            DropForeignKey("dbo.Game", "PublisherId", "dbo.Publisher");
            DropIndex("dbo.Genre", "IY_name");
            DropIndex("dbo.PlatformType", "IZ_type");
            DropIndex("dbo.Publisher", "IP_name");
            DropColumn("dbo.Comment", "ParentCommentId");
            RenameColumn(table: "dbo.Comment", name: "ParentComment_EntityId", newName: "ParentCommentId");
            RenameColumn(table: "dbo.Genre", name: "ParentGenre_EntityId", newName: "ParentGenre_Id");
            RenameIndex(table: "dbo.Comment", name: "IX_ParentComment_EntityId", newName: "IX_ParentCommentId");
            RenameIndex(table: "dbo.Genre", name: "IX_ParentGenre_EntityId", newName: "IX_ParentGenre_Id");
            DropPrimaryKey("dbo.Comment");
            DropPrimaryKey("dbo.Game");
            DropPrimaryKey("dbo.Genre");
            DropPrimaryKey("dbo.OrderDetail");
            DropPrimaryKey("dbo.Order");
            DropPrimaryKey("dbo.PlatformType");
            DropPrimaryKey("dbo.Publisher");
            CreateTable(
                "dbo.Ban",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Reason = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsCanceled = c.Boolean(nullable: false),
                        UserId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        PasswordSalt = c.String(),
                        IsLocked = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GenreTranslate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 65),
                        BaseEntityId = c.Guid(),
                        Language = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genre", t => t.BaseEntityId)
                .Index(t => t.Name, unique: true, name: "IY_name")
                .Index(t => t.BaseEntityId);
            
            CreateTable(
                "dbo.PlatformTypeTranslate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 65),
                        BaseEntityId = c.Guid(),
                        Language = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlatformType", t => t.BaseEntityId)
                .Index(t => t.Name, unique: true, name: "IZ_type")
                .Index(t => t.BaseEntityId);
            
            CreateTable(
                "dbo.PublisherTranslate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 65),
                        Description = c.String(),
                        BaseEntityId = c.Guid(),
                        Language = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publisher", t => t.BaseEntityId)
                .Index(t => t.Name, unique: true, name: "IP_name")
                .Index(t => t.BaseEntityId);
            
            CreateTable(
                "dbo.GameTranslate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        BaseEntityId = c.Guid(),
                        Language = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Game", t => t.BaseEntityId)
                .Index(t => t.BaseEntityId);
            
            CreateTable(
                "dbo.ManagerProfile",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Method = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleTranslate",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        BaseEntityId = c.Guid(),
                        Language = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.BaseEntityId)
                .Index(t => t.BaseEntityId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            AddColumn("dbo.Comment", "UserId", c => c.Guid());
            AddColumn("dbo.Comment", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Game", "FilePath", c => c.String());
            AddColumn("dbo.Game", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Genre", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetail", "IsPayed", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetail", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "ShippedDate", c => c.DateTime());
            AddColumn("dbo.Order", "IsShipped", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "UserId", c => c.Guid(nullable: false));
            AddColumn("dbo.Order", "IsPayed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.PlatformType", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Publisher", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Comment", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Game", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Genre", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.OrderDetail", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Order", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.PlatformType", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Publisher", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Comment", "Id");
            AddPrimaryKey("dbo.Game", "Id");
            AddPrimaryKey("dbo.Genre", "Id");
            AddPrimaryKey("dbo.OrderDetail", "Id");
            AddPrimaryKey("dbo.Order", "Id");
            AddPrimaryKey("dbo.PlatformType", "Id");
            AddPrimaryKey("dbo.Publisher", "Id");
            CreateIndex("dbo.Comment", "UserId");
            CreateIndex("dbo.Order", "UserId");
            AddForeignKey("dbo.Order", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comment", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Comment", "ParentCommentId", "dbo.Comment", "Id");
            AddForeignKey("dbo.Comment", "GameId", "dbo.Game", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GamesGenres", "GameId", "dbo.Game", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetail", "GameId", "dbo.Game", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GamesTypes", "GameId", "dbo.Game", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Genre", "ParentGenre_Id", "dbo.Genre", "Id");
            AddForeignKey("dbo.GamesGenres", "GenreId", "dbo.Genre", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GamesTypes", "PlatformTypeId", "dbo.PlatformType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Game", "PublisherId", "dbo.Publisher", "Id");
            DropColumn("dbo.Comment", "EntityId");
            DropColumn("dbo.Game", "EntityId");
            DropColumn("dbo.Game", "Name");
            DropColumn("dbo.Game", "Description");
            DropColumn("dbo.Genre", "EntityId");
            DropColumn("dbo.Genre", "Name");
            DropColumn("dbo.OrderDetail", "EntityId");
            DropColumn("dbo.Order", "EntityId");
            DropColumn("dbo.Order", "CustomerId");
            DropColumn("dbo.PlatformType", "EntityId");
            DropColumn("dbo.PlatformType", "Name");
            DropColumn("dbo.Publisher", "EntityId");
            DropColumn("dbo.Publisher", "Name");
            DropColumn("dbo.Publisher", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Publisher", "Description", c => c.String());
            AddColumn("dbo.Publisher", "Name", c => c.String(maxLength: 65));
            AddColumn("dbo.Publisher", "EntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.PlatformType", "Name", c => c.String(nullable: false, maxLength: 65));
            AddColumn("dbo.PlatformType", "EntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.Order", "CustomerId", c => c.Guid(nullable: false));
            AddColumn("dbo.Order", "EntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.OrderDetail", "EntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.Genre", "Name", c => c.String(nullable: false, maxLength: 65));
            AddColumn("dbo.Genre", "EntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.Game", "Description", c => c.String());
            AddColumn("dbo.Game", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Game", "EntityId", c => c.Guid(nullable: false));
            AddColumn("dbo.Comment", "EntityId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Game", "PublisherId", "dbo.Publisher");
            DropForeignKey("dbo.GamesTypes", "PlatformTypeId", "dbo.PlatformType");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.GamesGenres", "GenreId", "dbo.Genre");
            DropForeignKey("dbo.Genre", "ParentGenre_Id", "dbo.Genre");
            DropForeignKey("dbo.GamesTypes", "GameId", "dbo.Game");
            DropForeignKey("dbo.OrderDetail", "GameId", "dbo.Game");
            DropForeignKey("dbo.GamesGenres", "GameId", "dbo.Game");
            DropForeignKey("dbo.Comment", "GameId", "dbo.Game");
            DropForeignKey("dbo.Comment", "ParentCommentId", "dbo.Comment");
            DropForeignKey("dbo.Ban", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.User");
            DropForeignKey("dbo.RoleTranslate", "BaseEntityId", "dbo.Role");
            DropForeignKey("dbo.ManagerProfile", "Id", "dbo.User");
            DropForeignKey("dbo.Comment", "UserId", "dbo.User");
            DropForeignKey("dbo.GameTranslate", "BaseEntityId", "dbo.Game");
            DropForeignKey("dbo.PublisherTranslate", "BaseEntityId", "dbo.Publisher");
            DropForeignKey("dbo.PlatformTypeTranslate", "BaseEntityId", "dbo.PlatformType");
            DropForeignKey("dbo.Order", "UserId", "dbo.User");
            DropForeignKey("dbo.GenreTranslate", "BaseEntityId", "dbo.Genre");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.RoleTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.ManagerProfile", new[] { "Id" });
            DropIndex("dbo.GameTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PublisherTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PublisherTranslate", "IP_name");
            DropIndex("dbo.PlatformTypeTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.PlatformTypeTranslate", "IZ_type");
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.GenreTranslate", new[] { "BaseEntityId" });
            DropIndex("dbo.GenreTranslate", "IY_name");
            DropIndex("dbo.Comment", new[] { "UserId" });
            DropIndex("dbo.Ban", new[] { "UserId" });
            DropPrimaryKey("dbo.Publisher");
            DropPrimaryKey("dbo.PlatformType");
            DropPrimaryKey("dbo.Order");
            DropPrimaryKey("dbo.OrderDetail");
            DropPrimaryKey("dbo.Genre");
            DropPrimaryKey("dbo.Game");
            DropPrimaryKey("dbo.Comment");
            AlterColumn("dbo.Publisher", "Id", c => c.String());
            AlterColumn("dbo.PlatformType", "Id", c => c.String());
            AlterColumn("dbo.Order", "Id", c => c.String());
            AlterColumn("dbo.OrderDetail", "Id", c => c.String());
            AlterColumn("dbo.Genre", "Id", c => c.String());
            AlterColumn("dbo.Game", "Id", c => c.String());
            AlterColumn("dbo.Comment", "Id", c => c.String());
            DropColumn("dbo.Publisher", "IsDeleted");
            DropColumn("dbo.PlatformType", "IsDeleted");
            DropColumn("dbo.Order", "IsDeleted");
            DropColumn("dbo.Order", "IsPayed");
            DropColumn("dbo.Order", "UserId");
            DropColumn("dbo.Order", "IsShipped");
            DropColumn("dbo.Order", "ShippedDate");
            DropColumn("dbo.OrderDetail", "IsDeleted");
            DropColumn("dbo.OrderDetail", "IsPayed");
            DropColumn("dbo.Genre", "IsDeleted");
            DropColumn("dbo.Game", "IsDeleted");
            DropColumn("dbo.Game", "FilePath");
            DropColumn("dbo.Comment", "IsDeleted");
            DropColumn("dbo.Comment", "UserId");
            DropTable("dbo.UserRoles");
            DropTable("dbo.RoleTranslate");
            DropTable("dbo.Role");
            DropTable("dbo.ManagerProfile");
            DropTable("dbo.GameTranslate");
            DropTable("dbo.PublisherTranslate");
            DropTable("dbo.PlatformTypeTranslate");
            DropTable("dbo.GenreTranslate");
            DropTable("dbo.User");
            DropTable("dbo.Ban");
            AddPrimaryKey("dbo.Publisher", "EntityId");
            AddPrimaryKey("dbo.PlatformType", "EntityId");
            AddPrimaryKey("dbo.Order", "EntityId");
            AddPrimaryKey("dbo.OrderDetail", "EntityId");
            AddPrimaryKey("dbo.Genre", "EntityId");
            AddPrimaryKey("dbo.Game", "EntityId");
            AddPrimaryKey("dbo.Comment", "EntityId");
            RenameIndex(table: "dbo.Genre", name: "IX_ParentGenre_Id", newName: "IX_ParentGenre_EntityId");
            RenameIndex(table: "dbo.Comment", name: "IX_ParentCommentId", newName: "IX_ParentComment_EntityId");
            RenameColumn(table: "dbo.Genre", name: "ParentGenre_Id", newName: "ParentGenre_EntityId");
            RenameColumn(table: "dbo.Comment", name: "ParentCommentId", newName: "ParentComment_EntityId");
            AddColumn("dbo.Comment", "ParentCommentId", c => c.Guid());
            CreateIndex("dbo.Publisher", "Name", unique: true, name: "IP_name");
            CreateIndex("dbo.PlatformType", "Name", unique: true, name: "IZ_type");
            CreateIndex("dbo.Genre", "Name", unique: true, name: "IY_name");
            AddForeignKey("dbo.Game", "PublisherId", "dbo.Publisher", "EntityId");
            AddForeignKey("dbo.GamesTypes", "PlatformTypeId", "dbo.PlatformType", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.GamesGenres", "GenreId", "dbo.Genre", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.Genre", "ParentGenre_EntityId", "dbo.Genre", "EntityId");
            AddForeignKey("dbo.GamesTypes", "GameId", "dbo.Game", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.OrderDetail", "GameId", "dbo.Game", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.GamesGenres", "GameId", "dbo.Game", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.Comment", "GameId", "dbo.Game", "EntityId", cascadeDelete: true);
            AddForeignKey("dbo.Comment", "ParentComment_EntityId", "dbo.Comment", "EntityId");
        }
    }
}

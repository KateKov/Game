namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayedOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "IsPayed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "IsPayed");
        }
    }
}

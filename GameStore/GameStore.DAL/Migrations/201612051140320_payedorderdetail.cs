namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payedorderdetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetail", "IsPayed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetail", "IsPayed");
        }
    }
}

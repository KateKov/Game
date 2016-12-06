namespace GameStore.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second_migration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "CustomerId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "CustomerId", c => c.Guid(nullable: false));
        }
    }
}

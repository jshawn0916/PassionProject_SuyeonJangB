namespace PassionProject_SuyeonJangB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JourneyTravelersConnect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journeys", "TravelerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Journeys", "TravelerId");
            AddForeignKey("dbo.Journeys", "TravelerId", "dbo.Travelers", "TravelerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Journeys", "TravelerId", "dbo.Travelers");
            DropIndex("dbo.Journeys", new[] { "TravelerId" });
            DropColumn("dbo.Journeys", "TravelerId");
        }
    }
}

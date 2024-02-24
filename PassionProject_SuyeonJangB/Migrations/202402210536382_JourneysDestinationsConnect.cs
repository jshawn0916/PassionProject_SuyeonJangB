namespace PassionProject_SuyeonJangB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JourneysDestinationsConnect : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JourneyDestinations",
                c => new
                    {
                        Journey_JourneyId = c.Int(nullable: false),
                        Destination_DestinationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Journey_JourneyId, t.Destination_DestinationId })
                .ForeignKey("dbo.Journeys", t => t.Journey_JourneyId, cascadeDelete: true)
                .ForeignKey("dbo.Destinations", t => t.Destination_DestinationId, cascadeDelete: true)
                .Index(t => t.Journey_JourneyId)
                .Index(t => t.Destination_DestinationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JourneyDestinations", "Destination_DestinationId", "dbo.Destinations");
            DropForeignKey("dbo.JourneyDestinations", "Journey_JourneyId", "dbo.Journeys");
            DropIndex("dbo.JourneyDestinations", new[] { "Destination_DestinationId" });
            DropIndex("dbo.JourneyDestinations", new[] { "Journey_JourneyId" });
            DropTable("dbo.JourneyDestinations");
        }
    }
}

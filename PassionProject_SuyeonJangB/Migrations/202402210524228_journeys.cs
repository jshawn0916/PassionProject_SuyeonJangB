namespace PassionProject_SuyeonJangB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journeys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Journeys",
                c => new
                    {
                        JourneyId = c.Int(nullable: false, identity: true),
                        JourneyTitle = c.String(),
                        JourneyExplain = c.String(),
                    })
                .PrimaryKey(t => t.JourneyId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Journeys");
        }
    }
}

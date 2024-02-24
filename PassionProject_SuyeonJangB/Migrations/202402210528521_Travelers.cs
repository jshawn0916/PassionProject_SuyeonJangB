namespace PassionProject_SuyeonJangB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Travelers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Travelers",
                c => new
                    {
                        TravelerId = c.Int(nullable: false, identity: true),
                        TravelerFirstName = c.String(),
                        TravelerLastName = c.String(),
                        TravelerEmail = c.String(),
                    })
                .PrimaryKey(t => t.TravelerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Travelers");
        }
    }
}

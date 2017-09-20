namespace FollowUpTestClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EscalatedThreads",
                c => new
                    {
                        ThreadId = c.String(nullable: false, maxLength: 128),
                        cat_msalias = c.String(),
                        ThreadName = c.String(),
                        cat_URL = c.String(),
                        LastOP = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ThreadId);
            
            CreateTable(
                "dbo.FollowUpThreads",
                c => new
                    {
                        ThreadId = c.String(nullable: false, maxLength: 128),
                        cat_msalias = c.String(),
                        ThreadName = c.String(),
                        cat_URL = c.String(),
                        LastOP = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ThreadId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FollowUpThreads");
            DropTable("dbo.EscalatedThreads");
        }
    }
}

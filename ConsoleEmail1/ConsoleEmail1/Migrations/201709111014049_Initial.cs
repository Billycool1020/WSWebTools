namespace ConsoleEmail1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyThreads",
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
            DropTable("dbo.MyThreads");
        }
    }
}

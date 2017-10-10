namespace WSWebTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Engineers",
                c => new
                {
                    MSAlias = c.String(nullable: false, maxLength: 50),
                    Chinese = c.String(nullable: false, maxLength: 50),
                    OnBoardDate = c.DateTime(nullable: false),
                    GoLiveDate = c.DateTime(),
                    ReadinessPool = c.String(),
                    EID = c.Int(nullable: false),
                    DisplayName = c.String(nullable: false, maxLength: 50),
                    PhoneNumber = c.String(nullable: false, maxLength: 50),
                    Project = c.String(),
                    WSAlias = c.String(maxLength: 50),
                    SubTeamId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.MSAlias)
                .ForeignKey("dbo.Teams", t => t.SubTeamId)
                .Index(t => t.SubTeamId);

            CreateTable(
                "dbo.Teams",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TeamName = c.String(nullable: false, maxLength: 50),
                    OwnedTeamId = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    OwnerId = c.String(maxLength: 50),
                    ProductName = c.String(nullable: false),
                    SubTeamId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Engineers", t => t.OwnerId)
                .ForeignKey("dbo.Teams", t => t.SubTeamId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.SubTeamId);

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
                    ProductId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ThreadId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);

            CreateTable(
                "dbo.Forums",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    ForumName = c.String(),
                    ProductId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);

            CreateTable(
                "dbo.ThreadNotes",
                c => new
                    {
                        ThreadID = c.String(nullable: false, maxLength: 128),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ThreadID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Forums", "ProductId", "dbo.Products");
            DropForeignKey("dbo.FollowUpThreads", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "SubTeamId", "dbo.Teams");
            DropForeignKey("dbo.Products", "OwnerId", "dbo.Engineers");
            DropForeignKey("dbo.Engineers", "SubTeamId", "dbo.Teams");
            DropIndex("dbo.Forums", new[] { "ProductId" });
            DropIndex("dbo.FollowUpThreads", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "SubTeamId" });
            DropIndex("dbo.Products", new[] { "OwnerId" });
            DropIndex("dbo.Engineers", new[] { "SubTeamId" });
            DropTable("dbo.ThreadNotes");
            DropTable("dbo.Forums");
            DropTable("dbo.FollowUpThreads");
            DropTable("dbo.EscalatedThreads");
            DropTable("dbo.Products");
            DropTable("dbo.Teams");
            DropTable("dbo.Engineers");
        }
    }
}

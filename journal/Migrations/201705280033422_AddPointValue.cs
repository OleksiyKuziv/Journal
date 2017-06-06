namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPointValue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PointValues",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        SchoolID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Schools", t => t.SchoolID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.SchoolID);
            
            AddColumn("dbo.Points", "PointValue", c => c.Guid());
            AddColumn("dbo.Points", "PointValuew_ID", c => c.Guid());
            CreateIndex("dbo.Points", "PointValuew_ID");
            AddForeignKey("dbo.Points", "PointValuew_ID", "dbo.PointValues", "ID");
            DropColumn("dbo.Points", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Points", "Value", c => c.String());
            DropForeignKey("dbo.Points", "PointValuew_ID", "dbo.PointValues");
            DropForeignKey("dbo.PointValues", "SchoolID", "dbo.Schools");
            DropIndex("dbo.PointValues", new[] { "SchoolID" });
            DropIndex("dbo.PointValues", new[] { "ID" });
            DropIndex("dbo.Points", new[] { "PointValuew_ID" });
            DropColumn("dbo.Points", "PointValuew_ID");
            DropColumn("dbo.Points", "PointValue");
            DropTable("dbo.PointValues");
        }
    }
}

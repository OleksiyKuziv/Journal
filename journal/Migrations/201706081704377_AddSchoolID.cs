namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchoolID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PointLevels", "SchoolID", c => c.Guid());
            CreateIndex("dbo.PointLevels", "SchoolID");
            AddForeignKey("dbo.PointLevels", "SchoolID", "dbo.Schools", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PointLevels", "SchoolID", "dbo.Schools");
            DropIndex("dbo.PointLevels", new[] { "SchoolID" });
            DropColumn("dbo.PointLevels", "SchoolID");
        }
    }
}

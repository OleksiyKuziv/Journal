namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDbSetSchedules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        SubjectID = c.Guid(),
                        WeklyStartTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
            AddColumn("dbo.Subjects", "Schedule_ID", c => c.Guid());
            CreateIndex("dbo.Subjects", "Schedule_ID");
            AddForeignKey("dbo.Subjects", "Schedule_ID", "dbo.Schedules", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "Schedule_ID", "dbo.Schedules");
            DropIndex("dbo.Schedules", new[] { "ID" });
            DropIndex("dbo.Subjects", new[] { "Schedule_ID" });
            DropColumn("dbo.Subjects", "Schedule_ID");
            DropTable("dbo.Schedules");
        }
    }
}

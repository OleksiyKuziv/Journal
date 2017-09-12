namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schools", "TimeStamp", c => c.String());
            AlterColumn("dbo.Users", "RegisterDate", c => c.String());
            AlterColumn("dbo.Points", "Date", c => c.String());
            AlterColumn("dbo.Messages", "TimeStamp", c => c.String());
            AlterColumn("dbo.Schedules", "WeklyStartTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "WeklyStartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Points", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "RegisterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Schools", "TimeStamp", c => c.DateTime(nullable: false));
        }
    }
}

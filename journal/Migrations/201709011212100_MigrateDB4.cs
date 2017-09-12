namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "TimeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "TimeStamp", c => c.String());
        }
    }
}

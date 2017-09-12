namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Points", name: "PointValuew_ID", newName: "PointValueID");
            RenameIndex(table: "dbo.Points", name: "IX_PointValuew_ID", newName: "IX_PointValueID");
            DropColumn("dbo.Points", "PointValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Points", "PointValue", c => c.Guid());
            RenameIndex(table: "dbo.Points", name: "IX_PointValueID", newName: "IX_PointValuew_ID");
            RenameColumn(table: "dbo.Points", name: "PointValueID", newName: "PointValuew_ID");
        }
    }
}

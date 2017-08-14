namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDb3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ActivationKey", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ActivationKey");
        }
    }
}

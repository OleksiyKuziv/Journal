namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfirmEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ConfirmEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ConfirmEmail");
        }
    }
}

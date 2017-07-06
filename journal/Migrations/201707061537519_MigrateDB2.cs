namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactUs",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ContactUs", new[] { "ID" });
            DropTable("dbo.ContactUs");
        }
    }
}

namespace journal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Year = c.Int(nullable: false),
                        SchoolID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Schools", t => t.SchoolID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.SchoolID);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FullName = c.String(),
                        ShortName = c.String(),
                        TypeSchool = c.String(),
                        Degree = c.String(),
                        OwnerShip = c.String(),
                        ZipCode = c.Int(nullable: false),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        PhoneNumber = c.Long(nullable: false),
                        Email = c.String(),
                        Regulatory = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Age = c.Int(nullable: false),
                        UserRollID = c.Guid(),
                        Email = c.String(),
                        Phone = c.Long(nullable: false),
                        Password = c.String(),
                        ClassID = c.Guid(),
                        Degree = c.String(),
                        Info = c.String(),
                        RegisterDate = c.DateTime(nullable: false),
                        SchoolID = c.Guid(),
                        UserRole_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classes", t => t.ClassID)
                .ForeignKey("dbo.Schools", t => t.SchoolID)
                .ForeignKey("dbo.UserRoles", t => t.UserRole_ID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.ClassID)
                .Index(t => t.SchoolID)
                .Index(t => t.UserRole_ID);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Value = c.String(),
                        PointLevelID = c.Guid(),
                        SubjectID = c.Guid(),
                        UserId = c.Guid(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PointLevels", t => t.PointLevelID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ID, unique: true)
                .Index(t => t.PointLevelID)
                .Index(t => t.SubjectID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PointLevels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TeacherID = c.Guid(),
                        SubjectTypeID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubjectTypes", t => t.SubjectTypeID)
                .ForeignKey("dbo.Users", t => t.TeacherID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.TeacherID)
                .Index(t => t.SubjectTypeID);
            
            CreateTable(
                "dbo.SubjectTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
            CreateTable(
                "dbo.StudySubjects",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(),
                        SubjectID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.UserID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FromUserID = c.Guid(),
                        ToUserID = c.Guid(),
                        MessageTypeID = c.Guid(),
                        Text = c.String(),
                        Subject = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.FromUserID)
                .ForeignKey("dbo.MessageTypes", t => t.MessageTypeID)
                .ForeignKey("dbo.Users", t => t.ToUserID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.FromUserID)
                .Index(t => t.ToUserID)
                .Index(t => t.MessageTypeID);
            
            CreateTable(
                "dbo.MessageTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ToUserID", "dbo.Users");
            DropForeignKey("dbo.Messages", "MessageTypeID", "dbo.MessageTypes");
            DropForeignKey("dbo.Messages", "FromUserID", "dbo.Users");
            DropForeignKey("dbo.Users", "UserRole_ID", "dbo.UserRoles");
            DropForeignKey("dbo.StudySubjects", "UserID", "dbo.Users");
            DropForeignKey("dbo.StudySubjects", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Users", "SchoolID", "dbo.Schools");
            DropForeignKey("dbo.Points", "UserId", "dbo.Users");
            DropForeignKey("dbo.Subjects", "TeacherID", "dbo.Users");
            DropForeignKey("dbo.Subjects", "SubjectTypeID", "dbo.SubjectTypes");
            DropForeignKey("dbo.Points", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Points", "PointLevelID", "dbo.PointLevels");
            DropForeignKey("dbo.Users", "ClassID", "dbo.Classes");
            DropForeignKey("dbo.Classes", "SchoolID", "dbo.Schools");
            DropIndex("dbo.MessageTypes", new[] { "ID" });
            DropIndex("dbo.Messages", new[] { "MessageTypeID" });
            DropIndex("dbo.Messages", new[] { "ToUserID" });
            DropIndex("dbo.Messages", new[] { "FromUserID" });
            DropIndex("dbo.Messages", new[] { "ID" });
            DropIndex("dbo.UserRoles", new[] { "ID" });
            DropIndex("dbo.StudySubjects", new[] { "SubjectID" });
            DropIndex("dbo.StudySubjects", new[] { "UserID" });
            DropIndex("dbo.StudySubjects", new[] { "ID" });
            DropIndex("dbo.SubjectTypes", new[] { "ID" });
            DropIndex("dbo.Subjects", new[] { "SubjectTypeID" });
            DropIndex("dbo.Subjects", new[] { "TeacherID" });
            DropIndex("dbo.Subjects", new[] { "ID" });
            DropIndex("dbo.PointLevels", new[] { "ID" });
            DropIndex("dbo.Points", new[] { "UserId" });
            DropIndex("dbo.Points", new[] { "SubjectID" });
            DropIndex("dbo.Points", new[] { "PointLevelID" });
            DropIndex("dbo.Points", new[] { "ID" });
            DropIndex("dbo.Users", new[] { "UserRole_ID" });
            DropIndex("dbo.Users", new[] { "SchoolID" });
            DropIndex("dbo.Users", new[] { "ClassID" });
            DropIndex("dbo.Users", new[] { "ID" });
            DropIndex("dbo.Schools", new[] { "ID" });
            DropIndex("dbo.Classes", new[] { "SchoolID" });
            DropIndex("dbo.Classes", new[] { "ID" });
            DropTable("dbo.MessageTypes");
            DropTable("dbo.Messages");
            DropTable("dbo.UserRoles");
            DropTable("dbo.StudySubjects");
            DropTable("dbo.SubjectTypes");
            DropTable("dbo.Subjects");
            DropTable("dbo.PointLevels");
            DropTable("dbo.Points");
            DropTable("dbo.Users");
            DropTable("dbo.Schools");
            DropTable("dbo.Classes");
        }
    }
}

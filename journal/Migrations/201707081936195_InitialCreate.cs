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
                        PhoneNumber = c.String(),
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
                        Phone = c.String(),
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
                        PointValue = c.Guid(),
                        PointLevelID = c.Guid(),
                        SubjectID = c.Guid(),
                        UserId = c.Guid(),
                        Date = c.DateTime(nullable: false),
                        PointValuew_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PointLevels", t => t.PointLevelID)
                .ForeignKey("dbo.PointValues", t => t.PointValuew_ID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ID, unique: true)
                .Index(t => t.PointLevelID)
                .Index(t => t.SubjectID)
                .Index(t => t.UserId)
                .Index(t => t.PointValuew_ID);
            
            CreateTable(
                "dbo.PointLevels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                        SchoolID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Schools", t => t.SchoolID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.SchoolID);
            
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
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TeacherID = c.Guid(),
                        SubjectTypeID = c.Guid(),
                        Schedule_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubjectTypes", t => t.SubjectTypeID)
                .ForeignKey("dbo.Users", t => t.TeacherID)
                .ForeignKey("dbo.Schedules", t => t.Schedule_ID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.TeacherID)
                .Index(t => t.SubjectTypeID)
                .Index(t => t.Schedule_ID);
            
            CreateTable(
                "dbo.SubjectTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        SchoolID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Schools", t => t.SchoolID)
                .Index(t => t.ID, unique: true)
                .Index(t => t.SchoolID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "Schedule_ID", "dbo.Schedules");
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
            DropForeignKey("dbo.SubjectTypes", "SchoolID", "dbo.Schools");
            DropForeignKey("dbo.Points", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Points", "PointValuew_ID", "dbo.PointValues");
            DropForeignKey("dbo.PointValues", "SchoolID", "dbo.Schools");
            DropForeignKey("dbo.Points", "PointLevelID", "dbo.PointLevels");
            DropForeignKey("dbo.PointLevels", "SchoolID", "dbo.Schools");
            DropForeignKey("dbo.Users", "ClassID", "dbo.Classes");
            DropForeignKey("dbo.Classes", "SchoolID", "dbo.Schools");
            DropIndex("dbo.Schedules", new[] { "ID" });
            DropIndex("dbo.MessageTypes", new[] { "ID" });
            DropIndex("dbo.Messages", new[] { "MessageTypeID" });
            DropIndex("dbo.Messages", new[] { "ToUserID" });
            DropIndex("dbo.Messages", new[] { "FromUserID" });
            DropIndex("dbo.Messages", new[] { "ID" });
            DropIndex("dbo.ContactUs", new[] { "ID" });
            DropIndex("dbo.UserRoles", new[] { "ID" });
            DropIndex("dbo.StudySubjects", new[] { "SubjectID" });
            DropIndex("dbo.StudySubjects", new[] { "UserID" });
            DropIndex("dbo.StudySubjects", new[] { "ID" });
            DropIndex("dbo.SubjectTypes", new[] { "SchoolID" });
            DropIndex("dbo.SubjectTypes", new[] { "ID" });
            DropIndex("dbo.Subjects", new[] { "Schedule_ID" });
            DropIndex("dbo.Subjects", new[] { "SubjectTypeID" });
            DropIndex("dbo.Subjects", new[] { "TeacherID" });
            DropIndex("dbo.Subjects", new[] { "ID" });
            DropIndex("dbo.PointValues", new[] { "SchoolID" });
            DropIndex("dbo.PointValues", new[] { "ID" });
            DropIndex("dbo.PointLevels", new[] { "SchoolID" });
            DropIndex("dbo.PointLevels", new[] { "ID" });
            DropIndex("dbo.Points", new[] { "PointValuew_ID" });
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
            DropTable("dbo.Schedules");
            DropTable("dbo.MessageTypes");
            DropTable("dbo.Messages");
            DropTable("dbo.ContactUs");
            DropTable("dbo.UserRoles");
            DropTable("dbo.StudySubjects");
            DropTable("dbo.SubjectTypes");
            DropTable("dbo.Subjects");
            DropTable("dbo.PointValues");
            DropTable("dbo.PointLevels");
            DropTable("dbo.Points");
            DropTable("dbo.Users");
            DropTable("dbo.Schools");
            DropTable("dbo.Classes");
        }
    }
}

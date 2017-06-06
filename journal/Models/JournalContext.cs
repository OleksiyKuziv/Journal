using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace journal.Models
{
    public class JournalContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<PointLevel> PointLevels { get; set; }
        public DbSet<SubjectType> SubjectTypes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<School> Schools { get; set; }    
        public DbSet<PointValue> PointValues { get; set; }
        public DbSet<StudySubject> StudySubject { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
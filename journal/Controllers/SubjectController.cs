
using journal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using journal.ViewModels;
using journal.Helpers;
using journal.Filters;
using System.Security.Claims;

namespace journal.Controllers
{
    [Roles(Roles.Admin,Roles.Principle,Roles.Teacher,Roles.SuperAdmin)]
    public class SubjectController : Controller
    {
        // GET: Subject
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid TeacherRoll = Guid.Parse(Roles.Teacher);
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    TeacherSubjectViewModel teacherSubjectViewModel = new TeacherSubjectViewModel();
                    teacherSubjectViewModel.Teachers = db.Users
                        .Where(c => c.UserRollID == TeacherRoll&&(c.SchoolID==user.SchoolID||user.UserRollID==superAdmin))
                        .Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.FirstName + " " + s.LastName })
                        .ToList();
                    teacherSubjectViewModel.Subjects = db.Subjects
                        .Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.SubjectType.Name })
                        .ToList();
                    teacherSubjectViewModel.TeacherSubjectViewModels = db.Subjects
                        .Include(s => s.Teacher)
                        .Include(s => s.SubjectType)
                        .Where(c => c.Teacher.UserRollID == TeacherRoll&& (c.Teacher.SchoolID == user.SchoolID || user.UserRollID == superAdmin))
                        .Select(s => new SubjectViewModels()
                        {
                            ID = s.ID,
                            SelectedTeacher = s.Teacher.FirstName + " " + s.Teacher.LastName,
                            SelectedSubjectType = s.SubjectType.Name
                        })
                        .ToList();
                    return View(teacherSubjectViewModel);
                }
                return RedirectToAction("Login");
            }
        }

        public JsonResult Search(Guid? teacher, Guid? subject)
        {        
            using (JournalContext db = new JournalContext())
            {
                var newTeacherSubjectList = db.Subjects
                    .Include(s => s.Teacher)
                    .Include(s => s.SubjectType)
                    .Where(c=> (c.Teacher.ID == teacher && c.ID == subject) || (teacher== null && c.ID == subject)|| (c.Teacher.ID == teacher && subject == null) || (teacher==null&&subject==null))
                    .Select(s => new SubjectViewModels()
                {
                    ID = s.ID,
                    SelectedTeacher = s.Teacher.FirstName + " " + s.Teacher.LastName,
                    SelectedSubjectType = s.SubjectType.Name
                })
                .ToList();
                return Json(newTeacherSubjectList,JsonRequestBehavior.AllowGet);
            }
        }

        // GET: TeacherSubject/Details/5
        public ActionResult Details(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                var teacherSubject = db.Subjects
                    .Include(c => c.Teacher)
                    .Include(c => c.SubjectType)
                    .Where(c => c.ID == id)
                    .Select(c => new SubjectViewModels()
                    {
                        ID = c.ID,                        
                        SelectedTeacher = c.Teacher.FirstName + " " + c.Teacher.LastName,
                        SelectedSubjectType = c.SubjectType.Name
                    })
                    .FirstOrDefault();
                return View(teacherSubject);
            }
        }

        // GET: TeacherSubject/Create
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid teacherRoll = Guid.Parse(Roles.Teacher);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    Guid TeacherRoll = Guid.Parse(Roles.Teacher);
                    SubjectViewModels model = new SubjectViewModels();
                    model.Teachers = db.Users
                        .Where(c => c.UserRollID == teacherRoll && (c.SchoolID == user.SchoolID || user.UserRollID == superAdmin))
                        .Select(teacher => new SelectListItem() { Value = teacher.ID.ToString(), Text = teacher.FirstName + " " + teacher.LastName })
                        .ToList();
                    model.SubjectTypes = db.SubjectTypes
                        .Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name })
                        .ToList();
                    return View(model);
                }
                return RedirectToAction("Login");
            }
        }

        // POST: TeacherSubject/Create
        [HttpPost]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubjectViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                var teacherRoll = Guid.Parse(Roles.Teacher);
                if (ModelState.IsValid)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var idString = identity.Claims
                        .Where(c => c.Type == ClaimTypes.NameIdentifier)
                           .Select(c => c.Value).SingleOrDefault();
                    Guid id;
                    if (Guid.TryParse(idString, out id))
                    {
                        User user = db.Users.Find(id);
                        model.Teachers = db.Users
                            .Where(c => c.UserRollID == teacherRoll && (c.SchoolID == user.SchoolID || user.UserRollID == superAdmin))
                            .Select(teacher => new SelectListItem() { Value = teacher.ID.ToString(), Text = teacher.FirstName + " " + user.LastName })
                            .ToList();
                        model.SubjectTypes = db.SubjectTypes
                            .Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name })
                            .ToList();
                        Subject Subject = (Subject)model;
                        Subject.ID = Guid.NewGuid();
                        db.Subjects.Add(Subject);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Login");
                }
                return View(model);
            }
        }

        // GET: TeacherSubject/Edit/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid teacherRoll = Guid.Parse(Roles.Teacher);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid userId;
                if (Guid.TryParse(idString, out userId))
                {
                    User user = db.Users.Find(userId);
                    var subject = db.Subjects
                        .Include(c => c.Teacher)
                        .Include(c => c.SubjectType)
                        .Where(c => c.ID == id)
                        .Select(c => new SubjectViewModels()
                    {
                        ID = c.ID,
                        SelectedTeacher = c.Teacher.FirstName + " " + c.Teacher.LastName,
                        SelectedSubjectType = c.SubjectType.Name
                    }).FirstOrDefault();
                    subject.Teachers = db.Users
                        .Where(c => c.UserRollID == teacherRoll && (c.SchoolID == user.SchoolID || user.UserRollID == superAdmin))
                        .Select(teacher => new SelectListItem() { Value = teacher.ID.ToString(), Text = teacher.FirstName + " " + teacher.LastName })
                        .ToList();
                    subject.SubjectTypes = db.SubjectTypes
                        .Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name })
                        .ToList();
                    return View(subject);
                }
                return RedirectToAction("Login");
            }
        }

        // POST: TeacherSubject/Edit/5
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(SubjectViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid teacherRoll = Guid.Parse(Roles.Teacher);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    
                    model.Teachers = db.Users
                        .Where(c => c.UserRollID == teacherRoll && (c.SchoolID == user.SchoolID || user.UserRollID == superAdmin))
                        .Select(teacher => new SelectListItem() { Value = teacher.ID.ToString(), Text = teacher.FirstName + " " + teacher.LastName })
                        .ToList();
                    model.SubjectTypes = db.SubjectTypes
                        .Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name })
                        .ToList();
                    if (ModelState.IsValid)
                    {
                        Subject subject = db.Subjects.Find(model.ID);
                        subject.TeacherID = Guid.Parse(model.SelectedTeacher);
                        subject.SubjectTypeID = Guid.Parse(model.SelectedSubjectType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Login");
                }
                return View(model);
            }
        }

        // GET: TeacherSubject/Delete/5
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Delete(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                var subject = db.Subjects
                    .Include(c => c.Teacher)
                    .Include(c => c.SubjectType)
                    .Where(c => c.ID == id)
                    .Select(c => new SubjectViewModels()
                {
                    ID=c.ID,
                    SelectedTeacher=c.Teacher.FirstName+" "+c.Teacher.LastName,
                    SelectedSubjectType=c.SubjectType.Name
                })
                .FirstOrDefault();
                if (subject == null)
                {
                    return HttpNotFound();
                }
                return View(subject);
            }
        }

        // POST: TeacherSubject/Delete/5
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(SubjectViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                Subject subject = db.Subjects.Find(model.ID);
                var userToUpdateSubject = db.StudySubject
                    .Where(c => c.SubjectID == subject.ID);
                foreach (StudySubject studySubject in userToUpdateSubject)
                {
                    db.StudySubject.Remove(studySubject);
                }
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}

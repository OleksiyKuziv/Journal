using journal.Filters;
using journal.Helpers;
using journal.Models;
using journal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace journal.Controllers
{
    [Roles(Roles.Admin,Roles.MonitorGroup,Roles.Parent,Roles.Principle,Roles.Pupil,Roles.SuperAdmin)]
    public class StudySubjectController : Controller
    {        
        // GET: StudySubject
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid pupilRole = Guid.Parse(Roles.Pupil);
                Guid adminRole = Guid.Parse(Roles.Admin);
                Guid parentRole = Guid.Parse(Roles.Parent);
                Guid monitorGroup = Guid.Parse(Roles.MonitorGroup);
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid principle = Guid.Parse(Roles.Principle);
                StudyUserSubjectDropdownViewModel studyUserSubjectDropdown = new StudyUserSubjectDropdownViewModel();
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                    Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    studyUserSubjectDropdown.Users = db.Users
                        .Where(c => c.UserRollID == pupilRole&&(c.SchoolID==user.SchoolID||user.UserRollID==superAdmin))
                        .Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.FirstName + " " + s.LastName })
                        .ToList();
                    studyUserSubjectDropdown.Subjects = db.Subjects
                        .Where(c=>c.Teacher.SchoolID==user.SchoolID||user.UserRollID==superAdmin)
                        .Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.SubjectType.Name })
                        .ToList();
                    if (User.IsInRole(Roles.SuperAdmin))
                    {
                        studyUserSubjectDropdown.Schools = db.Schools
                            .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.ShortName })
                            .ToList();
                    }
                    studyUserSubjectDropdown.StudySubjects = db.StudySubject
                        .Include(c => c.User)
                        .Include(c => c.Subject)
                        .Where(c =>
                    ((user.UserRollID == adminRole||user.UserRollID==principle) && c.User.SchoolID == user.SchoolID)
                    ||
                    (c.UserID == user.ID && c.User.UserRollID == pupilRole && c.User.SchoolID == user.SchoolID)
                    ||
                    (user.UserRollID==parentRole&&c.User.SchoolID==user.SchoolID && c.User.ClassID == user.ClassID)
                    ||
                    (user.UserRollID==monitorGroup&&c.User.SchoolID==user.SchoolID&&c.User.ClassID==user.ClassID)
                    ||
                    user.UserRollID==superAdmin
                    )
                    .Select(c => new StudySubjectViewModel()
                    {
                        ID = c.ID,
                        SelectedSubject = c.Subject.SubjectType.Name,
                        SelectedUser = c.User.FirstName + " " + c.User.LastName

                    })
                    .ToList();
                  return View(studyUserSubjectDropdown);
                }
            }
            return RedirectToAction("Login");
        }

        // GET: StudySubject/Details/5
        public ActionResult Details(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {                
                var studySubject = db.StudySubject
                    .Include(c => c.User)
                    .Include(c => c.Subject)
                    .Where(c => c.ID == id)
                    .Select(c => new StudySubjectViewModel()
                {
                    ID=c.ID,
                    UserID = c.UserID,
                    SelectedUser = c.User.FirstName + " " + c.User.LastName,
                    SubjectID = c.SubjectID,
                    SelectedSubject = c.Subject.SubjectType.Name
                })
                .FirstOrDefault();
                return View(studySubject);
            }
        }

        // GET: StudySubject/Create
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle, Roles.Pupil,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            using (JournalContext db = new JournalContext())
            {                
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {

                    User user = db.Users.Find(id);
                    var pupilRole = Guid.Parse(Roles.Pupil);
                    Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                    StudySubjectViewModel studySubject = new StudySubjectViewModel();
                    studySubject.Users = db.Users
                        .Where(c => c.UserRollID == pupilRole && c.SchoolID == user.SchoolID && ((user.UserRollID == pupilRole && c.ID == user.ID) || (user.UserRollID != pupilRole)) || (user.UserRollID == superAdmin && c.UserRollID == pupilRole))
                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FirstName + " " + c.LastName })
                        .ToList();
                    studySubject.Subjects = db.Subjects
                        .Where(c => c.Teacher.SchoolID == user.SchoolID || user.UserRollID == superAdmin)
                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.SubjectType.Name })
                        .ToList();
                    return View(studySubject);

                }
                return RedirectToAction("Login");
            }
        }

        // POST: StudySubject/Create
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle, Roles.Pupil, Roles.SuperAdmin)]
        public ActionResult Create(StudySubjectViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid userID = Guid.Parse(model.SelectedUser);
                Guid subjectID = Guid.Parse(model.SelectedSubject);
                if (db.StudySubject.Where(c => c.UserID == userID && c.SubjectID == subjectID).Count() == 0)
                {
                    if (ModelState.IsValid)
                    {
                        StudySubject studySubject = (StudySubject)model;
                        studySubject.ID = Guid.NewGuid();
                        db.StudySubject.Add(studySubject);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["notice"] = "You have been already studying this subject.";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: StudySubject/Edit/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var identity = (ClaimsIdentity)User.Identity;
            var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();
            Guid UserId;
            if (Guid.TryParse(idString, out UserId))
            {
                using (JournalContext db = new JournalContext())
                {
                    User user = db.Users.Find(UserId);
                    Guid pupilRole = Guid.Parse(Roles.Pupil);
                    Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                    var studySubject = db.StudySubject
                        .Include(c => c.User)
                        .Include(c => c.Subject)
                        .Where(c => c.ID == id)
                        .Select(c => new StudySubjectViewModel()
                    {
                        ID=c.ID,
                        UserID = c.UserID,
                        SelectedUser = c.User.FirstName + " " + c.User.LastName,
                        SubjectID = c.SubjectID,
                        SelectedSubject = c.Subject.SubjectType.Name
                    })
                    .FirstOrDefault();
                    studySubject.Users = db.Users
                        .Where(c => (c.UserRollID == pupilRole && c.SchoolID == user.SchoolID)||(user.UserRollID==superAdmin&& c.UserRollID == pupilRole))
                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FirstName + " " + c.LastName })
                        .ToList();
                    studySubject.Subjects = db.Subjects
                        .Where(c => c.Teacher.SchoolID == user.SchoolID||user.UserRollID==superAdmin)
                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.SubjectType.Name })
                        .ToList();
                    return View(studySubject);
                }
            }
                return RedirectToAction("Login");
        }

        // POST: StudySubject/Edit/5
        [HttpPost]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(StudySubjectViewModel model)
        {

            var identity = (ClaimsIdentity)User.Identity;
            var idString = identity.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();
            Guid UserId;
            if (Guid.TryParse(idString, out UserId))
            {
                using (JournalContext db = new JournalContext())
                {
                    User user = db.Users.Find(UserId);
                    var pupilRole = Guid.Parse(Roles.Pupil);
                    model.Users = db.Users
                        .Where(c => c.UserRollID == pupilRole && c.SchoolID == user.SchoolID)
                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FirstName + " " + c.LastName })
                        .ToList();
                    model.Subjects = db.Subjects
                        .Where(c => c.Teacher.SchoolID == user.SchoolID)
                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.SubjectType.Name })
                        .ToList();
                    if (ModelState.IsValid)
                    {
                        StudySubject studySubject = db.StudySubject.Find(model.ID);
                        studySubject.UserID = Guid.Parse(model.SelectedUser);
                        studySubject.SubjectID = Guid.Parse(model.SelectedSubject);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(model);
                }
            }
            return RedirectToAction("Login");
        }

        // GET: StudySubject/Delete/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using (JournalContext db = new JournalContext())
            {               
                var studySubject = db.StudySubject
                    .Include(c => c.User)
                    .Include(c => c.Subject)
                    .Where(c => c.ID == id)
                    .Select(c => new StudySubjectViewModel()
                {
                    ID=c.ID,
                    UserID = c.UserID,
                    SelectedUser = c.User.FirstName + " " + c.User.LastName,
                    SubjectID = c.SubjectID,
                    SelectedSubject = c.Subject.SubjectType.Name
                })
                .FirstOrDefault();
                return View(studySubject);
            }
        }

        // POST: StudySubject/Delete/5
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult Delete(StudySubjectViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                StudySubject studySubject = db.StudySubject.Find(model.ID);
                db.StudySubject.Remove(studySubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public JsonResult Search(Guid? user, Guid? subject,Guid? school)
        {
            using (JournalContext db = new JournalContext())
            {
                var superAdmin = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User userCurrent = db.Users.Find(id);
                    var newTeacherSubjectList = db.StudySubject
                    .Include(s => s.User)
                    .Include(s => s.Subject)
                    .Where(c =>
                    ((c.UserID == user && c.Subject.ID == subject && c.User.SchoolID == school)
                    || (user == null && c.Subject.ID == subject && c.User.SchoolID == school)
                    || (c.UserID == user && subject == null && c.User.SchoolID == school)
                    || (c.UserID == user && c.Subject.ID == subject && school == null)
                    || (user == null && subject == null & c.User.SchoolID==school)
                    || (user == null && c.Subject.ID == subject && school==null)
                    || (c.UserID==user && subject == null && school == null)
                    || (user == null && subject == null && school == null) && userCurrent.UserRollID == superAdmin)
                    ||
                    ((c.UserID == user && c.Subject.ID == subject)
                    || (user == null && c.Subject.ID == subject)
                    || (c.UserID == user && subject == null)
                    || (user == null && subject == null) && c.User.SchoolID == userCurrent.SchoolID))
                    .Select(s => new StudySubjectViewModel()
                    {
                        ID = s.ID,
                        SelectedUser = s.User.FirstName + " " + s.User.LastName,
                        SelectedSubject = s.Subject.SubjectType.Name
                        
                    })
                .ToList();
                    return Json(newTeacherSubjectList, JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.DenyGet);
            }
        }
    }
}

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
    public class StudySubjectController : Controller
    {
        // GET: StudySubject
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                var pupilRole = Guid.Parse(Roles.Pupil);
                var adminRole = Guid.Parse(Roles.Admin);
                var parentRole = Guid.Parse(Roles.Parent);
                var monitorGroup = Guid.Parse(Roles.MonitorGroup);
                StudyUserSubjectDropdownViewModel studyUserSubjectDropdown = new StudyUserSubjectDropdownViewModel();
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                    Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    studyUserSubjectDropdown.Users = db.Users.Where(c => c.UserRollID == pupilRole).Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.FirstName + " " + s.LastName }).ToList();
                    studyUserSubjectDropdown.Subjects = db.Subjects.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.SubjectType.Name }).ToList();
                    studyUserSubjectDropdown.StudySubjects = db.StudySubject.Where(c =>
                    (user.UserRollID == adminRole && c.User.SchoolID == user.SchoolID)
                    ||
                    (c.UserID == user.ID && c.User.UserRollID == pupilRole && c.User.SchoolID == user.SchoolID)
                    ||
                    (user.UserRollID==parentRole&&c.User.SchoolID==user.SchoolID && c.User.ClassID == user.ClassID)
                    ||
                    (user.UserRollID==monitorGroup&&c.User.SchoolID==user.SchoolID&&c.User.ClassID==user.ClassID)
                    ).Include(c => c.User).Include(c => c.Subject).Select(c => new StudySubjectViewModel()
                    {
                        ID = c.ID,
                        SelectedSubject = c.Subject.SubjectType.Name,
                        SelectedUser = c.User.FirstName + " " + c.User.LastName

                    }).ToList();
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
                StudySubject studySubject = db.StudySubject.Find(id);
                StudySubjectViewModel model = new StudySubjectViewModel();
                model.UserID = studySubject.UserID;
                model.SelectedUser = db.Users.Where(c => c.ID == model.UserID).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();
                model.SubjectID = studySubject.SubjectID;
                model.SelectedSubject = db.Subjects.Where(c => c.ID == model.SubjectID).Include(c => c.SubjectType).Select(c => c.SubjectType.Name).FirstOrDefault();
                return View(model);
            }
        }

        // GET: StudySubject/Create
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle, Roles.Pupil,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();
            Guid id;
            if (Guid.TryParse(idString, out id))
            {             
                using (JournalContext db = new JournalContext())
                {
                    User user = db.Users.Find(id);
                    var pupilRole = Guid.Parse(Roles.Pupil);
                    StudySubjectViewModel studySubject = new StudySubjectViewModel();
                    studySubject.Users = db.Users.Where(c => c.UserRollID == pupilRole && c.SchoolID == user.SchoolID).Select(c => new SelectListItem() {Value=c.ID.ToString(),Text=c.FirstName+" "+c.LastName}).ToList();
                    studySubject.Subjects = db.Subjects.Where(c => c.Teacher.SchoolID == user.SchoolID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.SubjectType.Name }).ToList();

                    return View(studySubject);

                }
            }
            return RedirectToAction("Login");
        }

        // POST: StudySubject/Create
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle, Roles.Pupil, Roles.SuperAdmin)]
        public ActionResult Create(StudySubjectViewModel model)
        {
            using (JournalContext db = new JournalContext())
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
            return View(model);
        }

        // GET: StudySubject/Edit/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle)]
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
                    StudySubjectViewModel model = new StudySubjectViewModel();
                    StudySubject studySubject = db.StudySubject.Find(id);
                    var pupilRole = Guid.Parse(Roles.Pupil);
                    model.ID = studySubject.ID;
                    model.SubjectID = studySubject.SubjectID;
                    model.UserID = studySubject.UserID;
                    model.Users = db.Users.Where(c => c.UserRollID == pupilRole && c.SchoolID == user.SchoolID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FirstName + " " + c.LastName }).ToList();
                    model.Subjects = db.Subjects.Where(c => c.Teacher.SchoolID == user.SchoolID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.SubjectType.Name }).ToList();
                    model.SelectedUser = db.Users.Where(c => c.ID == model.UserID).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();
                    model.SelectedSubject = db.Subjects.Where(c => c.ID == model.SubjectID).Include(c => c.SubjectType).Select(c => c.SubjectType.Name).FirstOrDefault();
                    return View(model);
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
            var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();
            Guid UserId;
            if (Guid.TryParse(idString, out UserId))
            {
                using (JournalContext db = new JournalContext())
                {
                    User user = db.Users.Find(UserId);
                    var pupilRole = Guid.Parse(Roles.Pupil);
                    model.Users = db.Users.Where(c => c.UserRollID == pupilRole && c.SchoolID == user.SchoolID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FirstName + " " + c.LastName }).ToList();
                    model.Subjects = db.Subjects.Where(c => c.Teacher.SchoolID == user.SchoolID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.SubjectType.Name }).ToList();
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
                StudySubject studySubject = db.StudySubject.Find(id);
                StudySubjectViewModel model = new StudySubjectViewModel();
                model.ID = studySubject.ID;
                model.SubjectID = studySubject.SubjectID;
                model.UserID = studySubject.UserID;
                model.SelectedUser = db.Users.Where(c => c.ID == model.UserID).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();
                model.SelectedSubject = db.Subjects.Where(c => c.ID == model.SubjectID).Include(c => c.SubjectType).Select(c => c.SubjectType.Name).FirstOrDefault();
                return View(model);
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
        public JsonResult Search(Guid? user, Guid? subject)
        {
            using (JournalContext db = new JournalContext())
            {                
                var newTeacherSubjectList = db.StudySubject.Where(c => (c.UserID == user && c.Subject.ID == subject) || (user == null && c.Subject.ID == subject) || (c.UserID == user && subject == null) || (user == null && subject == null)).Include(s => s.User).Include(s => s.Subject).Select(s => new StudySubjectViewModel()
                {
                    ID = s.ID,
                    SelectedUser = s.User.FirstName + " " + s.User.LastName,
                    SelectedSubject = s.Subject.SubjectType.Name
                }).ToList();
                return Json(newTeacherSubjectList, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

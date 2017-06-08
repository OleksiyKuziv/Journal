
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

namespace journal.Controllers
{
    public class SubjectController : Controller
    {
        // GET: TeacherSubject
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                var TeacherRoll = Guid.Parse(Roles.Teacher);
                TeacherSubjectViewModel teacherSubjectViewModel = new TeacherSubjectViewModel();
                teacherSubjectViewModel.Teachers = db.Users.Where(c => c.UserRollID == TeacherRoll).Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.FirstName + " " + s.LastName }).ToList();
                teacherSubjectViewModel.Subjects = db.Subjects.Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.SubjectType.Name }).ToList();
                teacherSubjectViewModel.TeacherSubjectViewModels = db.Subjects.Where(c => c.Teacher.UserRollID == TeacherRoll).Include(s => s.Teacher).Include(s => s.SubjectType).Select(s => new SubjectViewModels()
                {
                    ID = s.ID,
                    SelectedTeacher = s.Teacher.FirstName + " " + s.Teacher.LastName,
                    SelectedSubjectType = s.SubjectType.Name
                    }).ToList();                
                return View(teacherSubjectViewModel);
            }
        }

        public JsonResult Search(Guid? teacher, Guid? subject)
        {        
            using (JournalContext db = new JournalContext())
            {
                var newTeacherSubjectList = db.Subjects.Where(c=> (c.Teacher.ID == teacher && c.ID == subject) || (teacher== null && c.ID == subject)|| (c.Teacher.ID == teacher && subject == null) || (teacher==null&&subject==null)).Include(s => s.Teacher).Include(s => s.SubjectType).Select(s => new SubjectViewModels()
                {
                    ID = s.ID,
                    SelectedTeacher = s.Teacher.FirstName + " " + s.Teacher.LastName,
                    SelectedSubjectType = s.SubjectType.Name
                }).ToList();
                return Json(newTeacherSubjectList,JsonRequestBehavior.AllowGet);
            }
        }

        // GET: TeacherSubject/Details/5
        public ActionResult Details(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                Subject teacherSubject = db.Subjects.Find(id);
                SubjectViewModels model = new SubjectViewModels();
                model.TeacherID = teacherSubject.TeacherID;
                model.SubjectTypeID = teacherSubject.SubjectTypeID;
                model.SelectedTeacher = db.Users.Where(c => c.ID == model.TeacherID).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();
                model.SelectedSubjectType = db.SubjectTypes.Where(c => c.ID == model.SubjectTypeID).Select(c => c.Name).FirstOrDefault();
                return View(model);
            }
        }

        // GET: TeacherSubject/Create
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            using (JournalContext db = new JournalContext())
            {
                var TeacherRoll = Guid.Parse(Roles.Teacher);
                SubjectViewModels model = new SubjectViewModels();
                model.Teachers = db.Users.Where(c => c.UserRollID == TeacherRoll).Select(user => new SelectListItem() { Value = user.ID.ToString(), Text = user.FirstName + " " + user.LastName }).ToList();
                model.SubjectTypes = db.SubjectTypes.Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name }).ToList();
                return View(model);
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
                if (ModelState.IsValid)
                {
                    var TeacherRoll = Guid.Parse(Roles.Teacher);
                    model.Teachers = db.Users.Where(c => c.UserRollID == TeacherRoll).Select(user => new SelectListItem() { Value = user.ID.ToString(), Text = user.FirstName + " " + user.LastName }).ToList();
                    model.SubjectTypes = db.SubjectTypes.Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name }).ToList();
                    Subject Subject = (Subject)model;
                    Subject.ID = Guid.NewGuid();
                    db.Subjects.Add(Subject);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                SubjectViewModels model = new SubjectViewModels();
                Subject teacherSubject = db.Subjects.Find(id);
                var TeacherRoll = Guid.Parse(Roles.Teacher);
                model.ID = teacherSubject.ID;
                model.TeacherID = teacherSubject.TeacherID;
                model.SubjectTypeID = teacherSubject.SubjectTypeID;
                model.Teachers = db.Users.Where(c => c.UserRollID == TeacherRoll).Select(user => new SelectListItem() { Value = user.ID.ToString(), Text = user.FirstName + " " + user.LastName }).ToList();
                model.SubjectTypes = db.SubjectTypes.Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name }).ToList();
                model.SelectedTeacher = db.Users.Where(c => c.ID == model.TeacherID).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();
                model.SelectedSubjectType = db.SubjectTypes.Where(c => c.ID == model.SubjectTypeID).Select(c => c.Name).FirstOrDefault();
                return View(model);
            }
        }

        // POST: TeacherSubject/Edit/5
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(SubjectViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    var TeacherRoll = Guid.Parse(Roles.Teacher);
                    model.Teachers = db.Users.Where(c => c.UserRollID == TeacherRoll).Select(user => new SelectListItem() { Value = user.ID.ToString(), Text = user.FirstName + " " + user.LastName }).ToList();
                    model.SubjectTypes = db.SubjectTypes.Select(subjectType => new SelectListItem() { Value = subjectType.ID.ToString(), Text = subjectType.Name }).ToList();
                    Subject subject = db.Subjects.Find(model.ID);
                    subject.TeacherID = Guid.Parse(model.SelectedTeacher);
                    subject.SubjectTypeID = Guid.Parse(model.SelectedSubjectType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
                Subject subject = db.Subjects.Find(id);
                SubjectViewModels model = new SubjectViewModels();
                model.ID = subject.ID;
                model.SubjectTypeID = subject.SubjectTypeID;
                model.TeacherID = subject.TeacherID;
                model.SelectedTeacher = db.Users.Where(c => c.ID == model.TeacherID).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();
                model.SelectedSubjectType = db.SubjectTypes.Where(c => c.ID == model.SubjectTypeID).Select(c => c.Name).FirstOrDefault();
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
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
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}

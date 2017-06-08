using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using journal.Models;
using journal.ViewModels;
using journal.Helpers;
using journal.Filters;
using System.Security.Claims;

namespace journal.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                var pupilRole = Guid.Parse(Roles.Pupil);
                var superAdmin = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    var newClassList = db.Classes.Where(c => (c.SchoolID == user.SchoolID)||(user.UserRollID==superAdmin)).Include(c => c.Users).Include(s => s.School).Select(s => new ClassViewModels()
                    {
                        ID = s.ID,
                        Name = s.Name,
                        Year = s.Year,
                        SelectedSchool = s.School.ShortName,
                        Users = s.Users.Select(c => new SelectListItem
                        {
                            Value = c.ID.ToString(),
                            Text = c.FirstName + " " + c.LastName
                        }
                        ).ToList()

                    }).ToList();
                    return View(newClassList);
                }
                return RedirectToAction("Login");
            }
        }

        // GET: Class/Details/5

        public ActionResult Details(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                var StudentRole = Guid.Parse(Roles.Pupil);
                ClassViewModels model = new ClassViewModels();
                Class newClass = db.Classes.Find(id);
                model.ID = newClass.ID;
                model.Name = newClass.Name;
                model.Year = newClass.Year;
                model.SchoolID = newClass.SchoolID;
                model.SelectedSchool = db.Schools.Where(c => c.ID == newClass.SchoolID).Select(x => x.ShortName).FirstOrDefault();
                model.Users = db.Users.Where(c => c.UserRollID == StudentRole && c.SchoolID == model.SchoolID && c.ClassID == model.ID).Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
                if (newClass == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }

        // GET: Class/Create
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            using (JournalContext db = new JournalContext())
            {
                var superAdminRole = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    if (user.UserRollID == superAdminRole)
                    {
                        ClassViewModels model = new ClassViewModels();
                        model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                        return View(model);
                    }
                }
                return View();
            }
        }

        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                var superAdminRole = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    if (user.UserRollID == superAdminRole)
                    {
                        model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                        model.SchoolID = Guid.Parse(model.SelectedSchool);
                    }
                    else
                    {
                        model.SchoolID = user.SchoolID;
                    }
                    if (ModelState.IsValid)
                    {
                        Class classes = (Class)model;
                        classes.ID = Guid.NewGuid();
                        classes.SchoolID = model.SchoolID;
                        db.Classes.Add(classes);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(model);
                }
                return RedirectToAction("Login");
            }
        }

        // GET: Class/Edit/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                var StudentRole = Guid.Parse(Roles.Pupil);
                ClassViewModels model = new ClassViewModels();
                Class newClass = db.Classes.Find(id);
                model.ID = newClass.ID;
                model.Name = newClass.Name;
                model.Year = newClass.Year;
                model.SchoolID = newClass.SchoolID;
                model.SelectedSchool = db.Schools.Where(c => c.ID == newClass.SchoolID).Select(x => x.ShortName).FirstOrDefault();
                model.Users = db.Users.Where(c => c.UserRollID == StudentRole && c.SchoolID == model.SchoolID && c.ClassID == model.ID).Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
                model.NewUsersList = db.Users.Where(c => c.UserRollID == StudentRole && c.SchoolID == model.SchoolID&&c.ClassID==null).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.FirstName + " " + c.LastName }).ToList();
                if (model == null)
                {
                    return HttpNotFound();
                }

                return View(model);
            }
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClassViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    Class newmodel = db.Classes.Find(model.ID);
                    newmodel.Name = model.Name;
                    newmodel.Year = model.Year;                       
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }

        // GET: Class/Delete/5
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Delete(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                var StudentRole = Guid.Parse(Roles.Pupil);
                ClassViewModels model = new ClassViewModels();
                Class newClass = db.Classes.Find(id);
                model.ID = newClass.ID;
                model.Name = newClass.Name;
                model.Year = newClass.Year;
                model.SchoolID = newClass.SchoolID;
                model.SelectedSchool = db.Schools.Where(c => c.ID == newClass.SchoolID).Select(x => x.ShortName).FirstOrDefault();
                model.Users = db.Users.Where(c => c.UserRollID == StudentRole && c.SchoolID == model.SchoolID && c.ClassID == model.ID).Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();

                if (newClass == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                Class newClass = db.Classes.Find(id);
                var userToUpdateClass = db.Users.Where(c => c.ClassID == newClass.ID);
                foreach(User user in userToUpdateClass)
                {
                    user.ClassID = null;
                }
                db.Classes.Remove(newClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public JsonResult DeleteUserWithClass(string selectedUser)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid selectedUserID = Guid.Parse(selectedUser);
                User deleteUser = db.Users.Find(selectedUserID);                
                var schoolID = deleteUser.SchoolID;
                var classID = deleteUser.ClassID;
                var pupilRole = Guid.Parse(Roles.Pupil);
                deleteUser.ClassID = null;
                db.SaveChanges();
                var newList = new List<object>();
                var newListOfUser = db.Users.Where(c=>c.UserRollID==pupilRole&&c.SchoolID==schoolID&&c.ClassID==classID).Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
                var newListOfUserWithoutClass = db.Users.Where(c => c.UserRollID == pupilRole && c.SchoolID == schoolID && c.ClassID == null).Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
                newList.Add(newListOfUser);
                newList.Add(newListOfUserWithoutClass);
                return Json(newList,JsonRequestBehavior.AllowGet);
            }
        }
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public JsonResult AddUserToClass(string selectedUser, string classID)
        {
            using (JournalContext db = new JournalContext())
            {
                var pupilRole = Guid.Parse(Roles.Pupil);
                var newSlectedUser = Guid.Parse(selectedUser);
                var selectedClassID = Guid.Parse(classID);
                User user = db.Users.Find(newSlectedUser);
                user.ClassID = selectedClassID;
                var schoolID = user.SchoolID;
                db.SaveChanges();
                var newListOfUser = db.Users.Where(c => c.UserRollID == pupilRole && c.SchoolID == schoolID && c.ClassID == selectedClassID).Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
                return Json(newListOfUser, JsonRequestBehavior.AllowGet);

            }
        }
    }
}

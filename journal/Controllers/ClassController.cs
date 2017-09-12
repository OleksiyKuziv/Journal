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
    [Authorize]
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid parentRole = Guid.Parse(Roles.Parent);
                Guid pupilRole = Guid.Parse(Roles.Pupil);
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid admin = Guid.Parse(Roles.Admin);
                Guid principle = Guid.Parse(Roles.Principle);
                Guid teacher = Guid.Parse(Roles.Teacher);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                       .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    var newClassList = db.Classes
                        .Include(c => c.Users).Include(s=>s.School)
                        .Where(c => (c.SchoolID == user.SchoolID&&c.ID==user.ClassID
                        &&(user.UserRollID==pupilRole||user.UserRollID==parentRole))
                    ||(user.UserRollID==superAdmin) 
                    || (c.SchoolID == user.SchoolID&&(user.UserRollID==admin||user.UserRollID==teacher||user.UserRollID==principle)))
                    .Select(s => new ClassViewModels()
                    {
                        ID = s.ID,
                        Name = s.Name,
                        Year = s.Year,
                        SelectedSchool = s.School.ShortName,
                        Users = s.Users
                        .Where(x=>x.UserRollID==pupilRole)
                        .Select(c => new SelectListViewModel
                        {
                           Value = c.ID.ToString(),
                            Text = c.FirstName + " " + c.LastName
                        }
                        )
                        .ToList()
                    })
                    .OrderBy(c=>c.Year)
                    .ThenBy(c=>c.Name)
                    .ToList();
                    return View(newClassList);
                }
                return RedirectToAction("Login");
            }
        }

        // GET: Class/Details/5

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            using (JournalContext db = new JournalContext())
            {
                
                Guid StudentRole = Guid.Parse(Roles.Pupil);
                var newClass = db.Classes
                    .Include(c => c.Users).Include(c => c.School)
                    .Where(c => c.ID == id).Select(c => new ClassViewModels()
                {
                    ID = c.ID,
                    Name = c.Name,
                    Year = c.Year,
                    SelectedSchool = c.School.ShortName,
                    Users = c.Users
                    .Where(x => x.UserRollID == StudentRole && x.SchoolID == c.SchoolID && x.ClassID == c.ID )
                    .Select(x => new SelectListViewModel() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName, ValueClass = x.ClassID.ToString() })
                    .ToList()
                }).FirstOrDefault();
                if (newClass == null)
                {
                    return HttpNotFound();
                }
                return View(newClass);
            }
        }

        // GET: Class/Create
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid superAdminRole = Guid.Parse(Roles.SuperAdmin);
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
                        model.Schools = db.Schools
                            .Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName })
                            .ToList();
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
                Guid superAdminRole = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (db.Classes.Where(c => c.Name == model.Name).Count() == 0)
                {
                    if (Guid.TryParse(idString, out id))
                    {
                        User user = db.Users.Find(id);
                        if (user.UserRollID == superAdminRole)
                        {
                            model.Schools = db.Schools
                                .Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName })
                                .ToList();
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
                else
                {
                    TempData["notice"] = "Current class are already exist";
                    return RedirectToAction("Index");
                }
               
            }
        }

        // GET: Class/Edit/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Edit(Guid? id)
        {
            using (JournalContext db = new JournalContext())
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                Guid StudentRole = Guid.Parse(Roles.Pupil);                
                var newClass = db.Classes
                    .Include(c => c.Users)
                    .Include(c => c.School)
                    .Where(c => c.ID == id)
                    .Select(c => new ClassViewModels()
                {
                    ID = c.ID,
                    Name = c.Name,
                    Year = c.Year,
                    SchoolID=c.SchoolID,
                    SelectedSchool = c.School.ShortName
                }).FirstOrDefault();
                newClass.Users = db.Users
                    .Where(x => x.UserRollID == StudentRole && x.SchoolID == newClass.SchoolID && (x.ClassID == newClass.ID || x.ClassID == null))
                    .Select(x => new SelectListViewModel() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName, ValueClass = x.ClassID.ToString() })
                    .ToList();
                return View(newClass);
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
        public ActionResult Delete(Guid? id)
        {
            using (JournalContext db = new JournalContext())
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                var StudentRole = Guid.Parse(Roles.Pupil);
                var newClass = db.Classes
                    .Include(c => c.Users)
                    .Include(c => c.School)
                    .Where(c => c.ID == id)
                    .Select(c => new ClassViewModels()
                {
                    ID = c.ID,
                    Name = c.Name,
                    Year = c.Year,
                    SelectedSchool = c.School.ShortName,
                    Users = c.Users
                    .Where(x => x.UserRollID == StudentRole && x.SchoolID == c.SchoolID && x.ClassID == c.ID)
                    .Select(x => new SelectListViewModel() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName, ValueClass = x.ClassID.ToString() })
                    .ToList()
                })
                .FirstOrDefault();
                if (newClass == null)
                {
                    return HttpNotFound();
                }
                return View(newClass);
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
        public JsonResult DeleteUserWithClass(Guid selectedUser)
        {
            using (JournalContext db = new JournalContext())
            {
                User user = db.Users.Find(selectedUser);
                Guid pupilRole = Guid.Parse(Roles.Pupil);
                Guid? classID = user.ClassID;
                user.ClassID = null;
                db.SaveChanges();
                var newListOfUser = db.Users
                    .Where(c => c.UserRollID == pupilRole && c.SchoolID == user.SchoolID && (c.ClassID == classID||c.ClassID==null))
                    .Select(x => new SelectListViewModel () { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName, ValueClass=x.ClassID.ToString()})
                    .ToList();
                return Json(newListOfUser, JsonRequestBehavior.AllowGet);
            }
        }
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public JsonResult AddUserToClass(Guid? selectedUser, Guid? classID)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid pupilRole = Guid.Parse(Roles.Pupil);              
                User user = db.Users.Find(selectedUser);
                user.ClassID = classID;
                Guid? schoolID = user.SchoolID;
                db.SaveChanges();
                var newListOfUser = db.Users
                    .Where(c => c.UserRollID == pupilRole && c.SchoolID == schoolID && c.ClassID == classID)
                    .Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.FirstName + " " + x.LastName })
                    .ToList();
                return Json(newListOfUser, JsonRequestBehavior.AllowGet);

            }
        }
    }
}

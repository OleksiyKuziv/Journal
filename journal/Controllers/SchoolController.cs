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
    public class SchoolController : Controller
    {
        // GET: School
        public ActionResult Index()
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
                    return View(db.Schools.Where(c => c.ID == user.SchoolID).ToList());
                }
            }
            return RedirectToAction("Login");
        }

        // GET: School/Details/5
        public ActionResult Details(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                School school = db.Schools.Find(id);
                if (school == null)
                {
                    return HttpNotFound();
                }

                return View(school);
            }
        }


        // GET: School/Create
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle)]
        public ActionResult Create()
        {
                return View();
        }

        // POST: School/Create
        [HttpPost]
        [Roles(Roles.Admin,Roles.Principle)]
        public ActionResult Create(SchoolViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    School school = (School)model;
                    school.ID = Guid.NewGuid();
                    school.TimeStamp = DateTime.Now;
                    db.Schools.Add(school);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }           
        

        // GET: School/Edit/5
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle)]
        public ActionResult Edit(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                School school = db.Schools.Find(id);
                if (school == null)
                {
                    return HttpNotFound();
                }
                return View(school);
            }
        }

        // POST: School/Edit/5
        [HttpPost]
        [Roles(Roles.Admin,Roles.Principle)]
        public ActionResult Edit(SchoolViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    School school = db.Schools.Find(model.ID);
                    school.FullName = model.FullName;
                    school.ShortName = model.ShortName;
                    school.TypeSchool = model.TypeSchool;
                    school.Degree = model.Degree;
                    school.OwnerShip = model.OwnerShip;
                    school.ZipCode = model.ZipCode;
                    school.Address1 = model.Address1;
                    school.Address2 = model.Address2;
                    school.PhoneNumber = model.PhoneNumber;
                    school.Email = model.Email;
                    school.Regulatory = model.Regulatory;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(model);

            }
        }            
        

        // GET: School/Delete/5
        [Roles(Roles.Admin,Roles.Principle)]
        public ActionResult Delete(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                School school = db.Schools.Find(id);
                if (school == null)
                {
                    return HttpNotFound();
                }
                return View(school);
            }
        }

        // POST: School/Delete/5
        [Roles(Roles.Admin,Roles.Principle)]
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(Guid id)
        {           
                using (JournalContext db = new JournalContext())
                {
                    School school = db.Schools.Find(id);
                    db.Schools.Remove(school);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }           
        }
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle)]
        public ActionResult CreatePointValue()
        {            
                return View();            
        }
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle)]
        public ActionResult CreatePointValue(PointValueViewModel model)
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
                    if (ModelState.IsValid)
                    {
                        PointValue pointValue = (PointValue)model;
                        pointValue.ID = Guid.NewGuid();
                        pointValue.SchoolID = user.SchoolID;
                        db.PointValues.Add(pointValue);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(model);
            }
        }

        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle)]
        public ActionResult CreatePointLevel()
        {
            return View();
        }
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle)]
        public ActionResult CreatePointLevel(PointLevelViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    PointLevel pointLevel = (PointLevel)model;
                    pointLevel.ID = Guid.NewGuid();
                    db.PointLevels.Add(pointLevel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }

    }
}

﻿using journal.Filters;
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
                var superAdminRole = Guid.Parse(Roles.SuperAdmin);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    return View(db.Schools.Where(c => (c.ID == user.SchoolID&&c.IsActive==true)||(user.UserRollID==superAdminRole && c.IsActive == true)).ToList());
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
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create()
        {
                return View();
        }

        // POST: School/Create
        [HttpPost]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create(SchoolViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    School school = (School)model;
                    school.ID = Guid.NewGuid();
                    school.TimeStamp = DateTime.Now;
                    school.IsActive = true;
                    db.Schools.Add(school);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }           
        

        // GET: School/Edit/5
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
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
        [Roles(Roles.Admin,Roles.Principle, Roles.SuperAdmin)]
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
        [Roles(Roles.SuperAdmin)]
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
        [Roles(Roles.SuperAdmin)]
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(Guid id)
        {           
                using (JournalContext db = new JournalContext())
                {
                    School school = db.Schools.Find(id);
                    school.IsActive = false;                    
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }           
        }
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult IndexPointValue()
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
                    User user = db.Users.Find(id);
                    var newPointValueList = db.PointValues.Where(c => (c.SchoolID == user.SchoolID) || (user.UserRollID == superAdmin)).Include(c => c.School).Select(c => new PointValueViewModel()
                    {
                        ID = c.ID,
                        Name = c.Name,
                        SelectedSchool = c.School.ShortName
                    }).ToList();
                    return View(newPointValueList);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult CreatePointValue()
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
                        PointValueViewModel model = new PointValueViewModel();
                        model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                        return View(model);
                    }
                }
                return View();
            }

        }
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult CreatePointValue(PointValueViewModel model)
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
                    User user = db.Users.Find(id);
                    if (ModelState.IsValid)
                    {
                        PointValue pointValue = (PointValue)model;
                        pointValue.ID = Guid.NewGuid();
                        if (user.UserRollID == superAdmin)
                        {
                            pointValue.SchoolID = Guid.Parse(model.SelectedSchool);
                        }
                        else
                        {
                            pointValue.SchoolID = user.SchoolID;
                        }
                        db.PointValues.Add(pointValue);
                        db.SaveChanges();
                        return RedirectToAction("IndexPointValue");
                    }
                }
                return View(model);
            }
        }
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult IndexPointLevel()
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
                    User user = db.Users.Find(id);
                    var newPointLevelList = db.PointLevels.Where(c => (c.SchoolID == user.SchoolID) || (user.UserRollID == superAdmin)).Include(c => c.School).Select(c => new PointLevelViewModels()
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Level=c.Level,
                        SelectedSchool = c.School.ShortName
                    }).ToList();
                    return View(newPointLevelList);
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult CreatePointLevel()
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
                        PointLevelViewModels model = new PointLevelViewModels();
                        model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                        return View(model);
                    }
                }
                return View();
            }
        }
        [HttpPost]
        [Roles(Roles.Admin, Roles.Principle,Roles.SuperAdmin)]
        public ActionResult CreatePointLevel(PointLevelViewModels model)
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
                    if (ModelState.IsValid)
                    {
                        PointLevel pointLevel = (PointLevel)model;
                        pointLevel.ID = Guid.NewGuid();
                        if (user.UserRollID == superAdminRole)
                        {
                            pointLevel.SchoolID = Guid.Parse(model.SelectedSchool);
                        }
                        else
                        {
                            pointLevel.SchoolID = user.SchoolID;
                        }
                        db.PointLevels.Add(pointLevel);
                        db.SaveChanges();
                        return RedirectToAction("IndexPointLevel");
                    }
                    return View(model);
                }
                return RedirectToAction("Login");
            }
        }

    }
}
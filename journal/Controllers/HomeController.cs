﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using journal.Models;
using journal.Helpers;

namespace journal.Controllers
{
    public class HomeController : Controller
    {
        JournalContext db = new JournalContext();
        public ActionResult Index()
        {
            return View(db.Schools.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Berezhany, school № 3";

            return View();
        }

        [HttpGet]
        public ActionResult Contact() {

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string fname, string lname, string description)
        {
            ViewBag.Message = "Thank you for your description.";

            return View();
        }
#region Roles
        [Authorize(Roles=Roles.Admin)]
        [HttpGet]
        public ActionResult UserRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRole(UserRole userRole)
        {
            userRole.Id = Guid.NewGuid();
            db.UserRoles.Add(userRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
#region School
        [HttpGet]
        public ActionResult AddSchool()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSchool(School school)
        {
            db.Schools.Add(school);
            db.SaveChanges();
            return View();
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public ActionResult SchoolEdit(int id=0)
        {
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        [HttpPost]
        public ActionResult SchoolEdit(School school)
        {
            School newschool = db.Schools.Find(school.Id);
            newschool.FullName = school.FullName;
            newschool.ShortName = school.ShortName;
            newschool.TypeSchool = school.TypeSchool;
            newschool.Degree = school.Degree;
            newschool.OwnerShip = school.OwnerShip;
            newschool.ZipCode = school.ZipCode;
            newschool.Address1 = school.Address1;
            newschool.Address2 = school.Address2;
            newschool.PhoneNumber = school.PhoneNumber;
            newschool.Email = school.Email;
            newschool.PrincipleId = school.PrincipleId;
            newschool.Regulatory = school.Regulatory;
            db.Entry(newschool).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
#endregion
        [HttpGet]
        public ActionResult CreateClass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateClass(Class newclass)
        {
            newclass.Id = Guid.NewGuid();
            db.Classes.Add(newclass);
            db.SaveChanges();
            return View();
        }

    }   
}
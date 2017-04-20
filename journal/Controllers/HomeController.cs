using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using journal.Models;

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

        [HttpGet]
        public ActionResult UserRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRole(UserRole userRole)
        {
            db.UserRoles.Add(userRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SchoolInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SchoolInfo(School school)
        {
            db.Schools.Add(school);
            db.SaveChanges();
            return View();
        }

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
            newschool.Name = school.Name;
            newschool.Description = school.Description;
            db.Entry(newschool).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }   
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using journal.Models;
using journal.Helpers;
using journal.ViewModels;

namespace journal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                return View(db.Schools.ToList());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Berezhany, school № 3";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Contact(string fname, string lname, string description)
        {
            ViewBag.Message = "Thank you for your description.";

            return View();
        }
        #region Roles
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public ActionResult UserRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult UserRole(UserRole userRole)
        {
            using (JournalContext db = new JournalContext())
            {
                userRole.Id = Guid.NewGuid();
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region School
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public ActionResult AddSchool()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult AddSchool(School school)
        {
            using (JournalContext db = new JournalContext())
            {
                school.Id = Guid.NewGuid();
                db.Schools.Add(school);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public ActionResult SchoolEdit(Guid Id)
        {
            using (JournalContext db = new JournalContext())
            {
                School school = db.Schools.Find(Id);
                if (school == null)
                {
                    return HttpNotFound();
                }
                return View(school);
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public ActionResult SchoolEdit(SchoolViewModel school)
        {
            using (JournalContext db = new JournalContext())
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
        }
#endregion
    }
}

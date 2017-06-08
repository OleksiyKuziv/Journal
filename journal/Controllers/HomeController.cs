using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using journal.Models;
using journal.Helpers;
using journal.ViewModels;
using journal.Filters;

namespace journal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                return View();
            }
        }

        public ActionResult About()
        {

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
        [Roles(Roles.SuperAdmin)]
        [HttpGet]
        public ActionResult UserRole()
        {
            return View();
        }
        [HttpPost]
        [Roles(Roles.SuperAdmin)]
        public ActionResult UserRole(UserRole userRole)
        {
            using (JournalContext db = new JournalContext())
            {
                userRole.ID = Guid.NewGuid();
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}

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
using System.Web.ModelBinding;

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
            ContactUsViewModel model = new ContactUsViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (JournalContext db = new JournalContext())
                {
                    ContactUs contactUs = new ContactUs();
                    contactUs.ID = Guid.NewGuid();
                    contactUs.FirstName = model.FirstName;
                    contactUs.LastName = model.LastName;
                    contactUs.Description = model.Description;
                    db.ContactUs.Add(contactUs);
                    db.SaveChanges();
                }
            }
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

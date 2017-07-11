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
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;

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
        public async Task<ActionResult> Contact(ContactUsViewModel model)
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

                    var body = "<p>Email From: {0} {1}</p><p>Message:</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress("kuzivo@ukr.net"));
                    message.From = new MailAddress("kuzivoles@gmail.com");
                    message.Subject = "Your email subject";
                    message.Body = string.Format(body, model.FirstName, model.LastName, model.Description);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "kuzivoles@gmail.com",  // replace with valid value
                            Password = "M@kintosh15091994"  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                        TempData["notice"] = "Thank you for your description";
                        return View();
                    }
                }
            }                    
            return View(model);
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

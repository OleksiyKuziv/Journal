using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using journal.Models;
using journal.ViewModels;
using journal.Helpers;

namespace journal.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {


        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";


            return View(new Object());
        }



        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(Guid? id)
        { 
            if(id==null)
            {
                return RedirectToAction("Login");
            }
            using (JournalContext db = new JournalContext())
            {
                User user = db.Users.Find(id);
                ChangePasswordViewModel model = new ChangePasswordViewModel();
                model.ID = user.ID;
                model.OldPassword = RijndaelForPassword.DecryptStringAES(user.Password, user.Email);

                return View(model);
            } 
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (JournalContext db = new JournalContext())
            {
                User user = db.Users.Find(model.ID);

                if (model.OldPassword != RijndaelForPassword.DecryptStringAES(user.Password, user.Email))
                {
                    TempData["notice"] = "Old password doesn't correct";
                    return View(model);
                }
                user.Password = RijndaelForPassword.EncryptStringAES(model.NewPassword, user.Email);
                await db.SaveChangesAsync();
            return RedirectToAction("AccountInfo","Account");
            }
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

       

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using journal.Models;
using journal.ViewModels;
using System.Data.Entity;
using journal.Helpers;
using System;

namespace journal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (JournalContext db = new JournalContext())
                {
                    User user = await db.Users/*.Include(u => u.UserRollId)*/.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Неправильний логін або пароль");
                    }
                    else
                    {
                        ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString(), ClaimValueTypes.String));
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email, ClaimValueTypes.String));
                        claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "OWIN Provider", ClaimValueTypes.String));
                        claim.AddClaim(new Claim(ClaimTypes.Role, user.UserRollID.ToString()));
                        //  if (user.UserRollId != null)
                        //  claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRollId.Name, ClaimValueTypes.String));

                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
        }

        public ActionResult LogOff() {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            using (JournalContext db = new JournalContext())
            {
                model.Roles = db.UserRoles.Select(role => new SelectListItem() { Value = role.ID.ToString(), Text = role.Name }).ToList();
                model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                model.Classes = db.Classes.Select(@class => new SelectListItem() { Value = @class.ID.ToString(), Text = @class.Name }).ToList();
            }
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {

                if (ModelState.IsValid)
                {
                    if (db.Users.Any(u => u.Email == model.Email))
                    {
                        model.Roles = db.UserRoles.Select(role => new SelectListItem() { Value = role.ID.ToString(), Text = role.Name }).ToList();
                        model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                        model.Classes = db.Classes.Select(@class => new SelectListItem() { Value = @class.ID.ToString(), Text = @class.Name }).ToList();
                        ModelState.AddModelError("Email", "Such email is used. Please choose another.");
                        return View(model);
                    }
                    User user = (User)model;
                    user.ID = Guid.NewGuid();
                    user.RegisterDate = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();


                    return RedirectToAction("Index", "Home");

                }
                model.Roles = db.UserRoles.Select(role => new SelectListItem() { Value = role.ID.ToString(), Text = role.Name }).ToList();
                model.Schools = db.Schools.Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                model.Classes = db.Classes.Select(@class => new SelectListItem() { Value = @class.ID.ToString(), Text = @class.Name }).ToList();
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            return View(/*result.Succeeded*/ true ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return View("ForgotPasswordConfirmation");
                //}


            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AccountInfo()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();
            Guid id;
            if (Guid.TryParse(idString, out id))
            {
                using (JournalContext db = new JournalContext())
                {
                    UserViewModels model = new UserViewModels();
                    User user = db.Users.Find(id);
                    model.ID = user.ID;
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.Age = user.Age;
                    model.UserRollID = user.UserRollID;
                    model.Email = user.Email;
                    model.Phone = user.Phone;
                    model.Password = user.Password;
                    model.ClassID = user.ClassID;
                    model.Degree = user.Degree;
                    model.Info = user.Info;
                    model.UserRollSelected = db.UserRoles.Where(c => c.ID == model.UserRollID).Select(x => x.Name).FirstOrDefault();
                    model.ClassSelected = db.Classes.Where(c => c.ID == model.ClassID).Select(x => x.Name).FirstOrDefault();
                    return View(model);
                }
            }
            AuthenticationManager.SignOut();
            return View("LogIn");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditAccount(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                UserViewModels model = new UserViewModels();
                User user = db.Users.Find(id);
                model.ID = user.ID;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Age = user.Age;
                model.UserRollID = user.UserRollID;
                model.Email = user.Email;
                model.Phone = user.Phone;
                model.Password = user.Password;
                model.ClassID = user.ClassID;
                model.Degree = user.Degree;
                model.Info = user.Info;
                model.ClassSelected = db.Classes.Where(c => c.ID == model.ClassID).Select(x => x.Name).FirstOrDefault();
                model.UserRollSelected = db.UserRoles.Where(c => c.ID == model.UserRollID).Select(x => x.Name).FirstOrDefault();
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditAccount(UserViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    User user = db.Users.Find(model.ID);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Age = model.Age;
                    user.Degree = model.Degree;
                    user.Email = model.Email;
                    user.Info = model.Info;
                    user.Phone = model.Phone;
                    user.ClassID = model.ClassID;
                    user.Password = model.Password;
                    user.UserRollID = model.UserRollID;
                    
                    db.SaveChanges();
                    return RedirectToAction("AccountInfo");
                }
                return View(model);
            }
        }
        [AllowAnonymous]
        public JsonResult SearchClass(string selectedSchool)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid SelectedSchool = Guid.Parse(selectedSchool);
                var @classList = db.Classes.Where(c => c.SchoolID == SelectedSchool).Include(c => c.School).Select(c => new ClassViewModels
                {
                    ID=c.ID,
                    Name=c.Name,
                    Year=c.Year,
                    SelectedSchool=c.School.ShortName
                }).ToList();


            return Json(@classList, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
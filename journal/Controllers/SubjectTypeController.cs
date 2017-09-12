using journal.Filters;
using journal.Helpers;
using journal.Models;
using journal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace journal.Controllers
{
    [Authorize]
    public class SubjectTypeController : Controller
    {
        // GET: Subject
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                SubjectTypeWithDropdownViewModel model = new SubjectTypeWithDropdownViewModel();
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid UserId;
                if (Guid.TryParse(idString, out UserId))
                {
                    User user = db.Users.Find(UserId);
                    model.SubjectTypes = db.SubjectTypes
                        .Include(c => c.School)
                        .Where(c => c.SchoolID == user.SchoolID || user.UserRollID == superAdmin)
                        .Select(c => new SubjectTypeViewModels()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            SelectedSchool = c.School.ShortName
                        })
                        .OrderBy(c=>c.Name)
                        .ToList();
                    if (User.IsInRole(Roles.SuperAdmin))
                    {
                        model.Schools = db.Schools.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.ShortName }).ToList();
                    }
                    return View(model);
                }
                return RedirectToAction("Login");
            }
        }
        // GET: Subject/Details/5
        [HttpGet]
        public ActionResult Details(Guid id)
        {
            using (JournalContext db = new JournalContext()) {
                SubjectType subjectType = db.SubjectTypes.Find(id);
                if (subjectType == null)
                {
                    return HttpNotFound();
                }
                return View(subjectType);
            }
        }

        // GET: Subject/Create
        [HttpGet]
        [Roles(Roles.Admin, Roles.Teacher, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult Create()
        {
            using (JournalContext db = new JournalContext())
            {
                SubjectTypeViewModels model = new SubjectTypeViewModels();
                model.Schools = db.Schools
                    .Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                return View(model);
            }
        }

        // POST: Subject/Create
        [HttpPost]
        [Roles(Roles.Admin, Roles.Teacher, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult Create(SubjectTypeViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid UserId;
                if (Guid.TryParse(idString, out UserId))
                {
                    User user = db.Users.Find(UserId);
                    if (db.SubjectTypes.Where(c => c.Name == model.Name&&c.SchoolID==user.SchoolID).Count() == 0)
                    {
                        if (ModelState.IsValid)
                        {
                            SubjectType subjectType = (SubjectType)model;
                            subjectType.ID = Guid.NewGuid();

                            if (user.UserRollID == Guid.Parse(Roles.SuperAdmin))
                            {
                                subjectType.SchoolID = Guid.Parse(model.SelectedSchool);
                            }
                            else
                            {
                                subjectType.SchoolID = user.SchoolID;
                            }
                            db.SubjectTypes.Add(subjectType);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        model.Schools = db.Schools
                       .Select(school => new SelectListItem() { Value = school.ID.ToString(), Text = school.ShortName }).ToList();
                        return View(model);
                    }
                    else
                    {
                        TempData["notice"] = "The subject has already exsisted";
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Login");
            }
        }

        // GET: Subject/Edit/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult Edit(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                SubjectTypeViewModels model = new SubjectTypeViewModels();
                SubjectType subjectType = db.SubjectTypes.Find(id);
                model.ID = subjectType.ID;
                model.Name = subjectType.Name;
                model.Description = subjectType.Description;
                if (subjectType == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }


        // POST: Subject/Edit/5
        [HttpPost]
        [Roles(Roles.Principle, Roles.Admin, Roles.SuperAdmin)]
        public ActionResult Edit(SubjectTypeViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    SubjectType subjectType = db.SubjectTypes.Find(model.ID);
                    subjectType.Name = model.Name;
                    subjectType.Description = model.Description;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Subject/Delete/5
        [HttpGet]
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        public ActionResult Delete(Guid id)
        {
            using (JournalContext db = new JournalContext()) {
                SubjectTypeViewModels model = new SubjectTypeViewModels();
                SubjectType subjectType = db.SubjectTypes.Find(id);
                model.ID = subjectType.ID;
                model.Name = subjectType.Name;
                model.Description = subjectType.Description;
                if (model == null)
                {
                    return HttpNotFound();
                }

                return View(model);
            }
        }

        // POST: Subject/Delete/5
        [Roles(Roles.Admin, Roles.Principle, Roles.SuperAdmin)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(Guid id)
        {
            using (JournalContext db = new JournalContext())
            {
                SubjectType subjectType = db.SubjectTypes.Find(id);
                db.SubjectTypes.Remove(subjectType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public JsonResult Search(Guid? school)
        {
            using (JournalContext db = new JournalContext())
            {
                var newSubjectTypeList = db.SubjectTypes
                    .Include(c=>c.School)
                    .Where(c=>c.SchoolID==school||school==null)
                    .Select(c=> new SubjectTypeViewModels()
                    {
                        ID=c.ID,
                        Name=c.Name,
                        Description=c.Description,
                        SelectedSchool=c.School.ShortName
                    })
                    .ToList();
                return Json(newSubjectTypeList, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

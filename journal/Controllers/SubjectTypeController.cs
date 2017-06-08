using journal.Filters;
using journal.Helpers;
using journal.Models;
using journal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace journal.Controllers
{
    public class SubjectTypeController : Controller
    {
        // GET: Subject
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                return View(db.SubjectTypes.ToList());
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
        [Roles(Roles.Admin,Roles.Teacher,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subject/Create
        [HttpPost]
        [Roles(Roles.Admin,Roles.Teacher,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Create(SubjectTypeViewModels model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (ModelState.IsValid)
                {
                    SubjectType subjectType = (SubjectType)model;
                    subjectType.ID = Guid.NewGuid();
                    db.SubjectTypes.Add(subjectType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(model);
            }
        }

        // GET: Subject/Edit/5
        [HttpGet]
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
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
        [Roles(Roles.Principle,Roles.Admin,Roles.SuperAdmin)]
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
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        public ActionResult Delete(Guid id)
        {
            using (JournalContext db = new JournalContext()) {
                SubjectTypeViewModels model = new SubjectTypeViewModels();
                SubjectType subjectType =db.SubjectTypes.Find(id);
                model.ID =subjectType.ID;
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
        [Roles(Roles.Admin,Roles.Principle,Roles.SuperAdmin)]
        [HttpPost,ActionName("Delete")]
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
    }
}

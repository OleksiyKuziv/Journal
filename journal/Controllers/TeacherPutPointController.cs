using journal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Claims;
using journal.Helpers;
using journal.ViewModels;

namespace journal.Controllers
{
    public class TeacherPutPointController : Controller
    {
        // GET: TeacherPutPoin
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid? teacher = Guid.Parse(Roles.Teacher);
                Guid? admin = Guid.Parse(Roles.Admin);
                Guid? superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid? principle = Guid.Parse(Roles.Principle);
                Guid? pupil = Guid.Parse(Roles.Pupil);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User currentUser = db.Users.Find(id);
                    TeacherPutPointViewModel model = new TeacherPutPointViewModel();                   
                    model.PointLevels = db.PointLevels
                        .Where(c => c.SchoolID == currentUser.SchoolID)
                        .OrderBy(c => c.Level)
                        .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name })
                        .ToList();                   
                    model.Subjects = db.Subjects
                        .Where(c => c.SubjectType.SchoolID == currentUser.SchoolID)
                        .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.SubjectType.Name })
                        .OrderBy(c => c.Text)
                        .ToList();
                    //model.PointsView = db.Points
                    //.Include(c => c.User)
                    //.Include(c => c.PointValue)
                    //.Include(c => c.PointLevel)
                    //.Where(c => c.User.SchoolID == currentUser.SchoolID)
                    //.Select(c => new PointViewModels()
                    //{
                    //    ID = c.ID,
                    //    UserID = c.User.SchoolID,
                    //    SelectedPoinLevel = c.PointLevel.Name,
                    //    SelectedPointValue = c.PointValue.Name,
                    //    SelectedSubject = c.Subject.SubjectType.Name,
                    //    SelectedUser = c.User.LastName+" "+c.User.FirstName,
                    //    Date=c.Date
                    //}).ToList();
                    return View(model);
                }
                return RedirectToAction("Login");
            }
        }
        public JsonResult Search(Guid? subjectid)
        {
            using (JournalContext db = new JournalContext())
            {
                TeacherPutPointViewModel model1 = new TeacherPutPointViewModel();
                PointViewModels model = new PointViewModels();
                if (subjectid == null)
                {
                    return Json(model,JsonRequestBehavior.AllowGet);
                } 
                model1.Users= db.StudySubject
                    .Where(c => c.SubjectID == subjectid)
                    .Select(c => new SelectListViewModel() { Value = c.UserID.ToString(), Text = c.User.LastName + " " + c.User.FirstName, SchoolId=c.User.SchoolID.ToString() })
                    .OrderBy(c => c.Text)
                    .ToList();
                Guid schoolID=Guid.Parse(db.StudySubject.Where(c => c.SubjectID == subjectid).Select(c => c.User.SchoolID.ToString()).FirstOrDefault());
                model.PointValues = db.PointValues
                    .OrderBy(c=>c.Value)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();

                
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult AddPoint(object userList,Guid? subjectId, Guid? pointLevel)
        //{
        //    using (JournalContext db = new JournalContext())
        //    {
        //        //foreach(var i in userList)

        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //}
        [HttpPost]
        public ActionResult AddPoint(UserPointViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                for (int i=0;i<model.userList.Count();i++)
                {
                    Point point = new Point();
                    point.ID = Guid.NewGuid();
                    point.PointLevelID = Guid.Parse(model.pointLevel);
                    point.SubjectID = Guid.Parse(model.subjectid);
                    point.UserId = Guid.Parse(model.userList[i].user);
                    point.PointValueID = Guid.Parse(model.userList[i].pointValue);
                    point.Date = DateTime.Now;
                    db.Points.Add(point);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }             
        }
    }
}

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
using journal.Filters;

namespace journal.Controllers
{
    public class TeacherPutPointController : Controller
    {
        // GET: TeacherPutPoin
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                Guid admin = Guid.Parse(Roles.Admin);
                Guid principle = Guid.Parse(Roles.Principle);
                Guid superAdmin = Guid.Parse(Roles.SuperAdmin);
                Guid teacher = Guid.Parse(Roles.Teacher);
                Guid pupil = Guid.Parse(Roles.Pupil);
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User currentUser = db.Users.Find(id);
                    TeacherPutPointViewModel model = new TeacherPutPointViewModel();
                    if (currentUser.UserRollID == Guid.Parse(Roles.Pupil) || currentUser.UserRollID == Guid.Parse(Roles.Parent))
                    {
                        model.Subjects = db.StudySubject.Where(c => c.UserID == currentUser.ID)
                            .Select(c => new SelectListViewModel() { Value = c.SubjectID.ToString(), Text = c.Subject.SubjectType.Name })
                            .OrderBy(c => c.Text)
                            .ToList();
                    }
                    else
                    {
                        model.Subjects = db.Subjects
                            .Where(c => (c.Teacher.UserRollID == teacher && c.TeacherID == currentUser.ID)
                            ||
                            (c.SubjectType.SchoolID == currentUser.SchoolID && (currentUser.UserRollID == admin || currentUser.UserRollID == principle))
                            ||
                            currentUser.UserRollID == superAdmin
                            )
                            .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.SubjectType.Name })
                            .OrderBy(c => c.Text)
                            .ToList();
                    }
                    model.CurrentRole = currentUser.UserRollID;
                    return View(model);
                }
                return RedirectToAction("Login");
            }
        }
        public JsonResult Search(Guid? subjectid)
        {
            using (JournalContext db = new JournalContext())
            {
                TeacherPutPointViewModel model = new TeacherPutPointViewModel();                
                if (subjectid == null)
                {
                    return Json(model,JsonRequestBehavior.AllowGet);
                }
                if (db.Points.Where(c => c.SubjectID == subjectid).Count() == 0)
                {
                    model.Users = db.StudySubject
                        .Where(c => c.SubjectID == subjectid)
                        .Select(c => new SelectListViewModel() { Value = c.UserID.ToString(), Text = c.User.LastName + " " + c.User.FirstName, SchoolId = c.User.SchoolID.ToString() })
                        .OrderBy(c => c.Text).ToList();
                }
                else
                {
                    model.PointsView = db.Points
                        .Include(c => c.PointLevel)
                        .Include(c => c.PointValue)
                        .Include(c => c.User)
                        .Where(c => c.SubjectID == subjectid)
                        .Select(c => new PointViewModels()
                        {
                            ID = c.ID,
                            SelectedUser = c.User.LastName + " " + c.User.FirstName,
                            UserID = c.UserId,
                            SelectedPointLevel = c.PointLevel.Name,
                            PointLevelID = c.PointLevelID,
                            SelectedPointValue = c.PointValue.Name,
                            PointValueID = c.PointValueID,
                            Date = c.Date
                        })
                        .OrderBy(c => c.PointLevelID)
                        .ThenBy(c=>c.Date)
                        .ToList();
                    model.PointsView.ForEach(c => c.StrDate = c.Date.ToString("dd-MM-yyyy"));

                }  
               
                Guid schoolID=Guid.Parse(db.StudySubject.Where(c => c.SubjectID == subjectid).Select(c => c.User.SchoolID.ToString()).FirstOrDefault());
                model.PointValues = db.PointValues
                    .OrderBy(c=>c.Value)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                model.PointLevels = db.PointLevels
                    .OrderBy(c => c.Level)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        [Roles(Roles.Teacher)]
        public JsonResult SavePoint(UserPointViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                Guid pointLevel = Guid.Parse(model.pointLevel);
                Guid subjectid = Guid.Parse(model.subjectid);
                for (int i = 0; i < model.userList.Count(); i++)
                {
                    Guid user = Guid.Parse(model.userList[i].user);
                    Guid value = Guid.Parse(model.userList[i].pointValue);
                    var point = db.Points.Where(c => (c.PointLevelID == pointLevel && c.SubjectID == subjectid && c.UserId==user))
                        .Select(c => new PointViewModels()
                    {
                        ID = c.ID,
                        Date=c.Date
                    }).ToList();
                    point.ForEach(c => c.StrDate = c.Date.ToString("dd-MM-yyyy"));
                    for (int j = 0; j < point.Count(); j++)
                    {
                        if (point[j].StrDate == model.dataEdit)
                        {
                            Point editpoint = db.Points.Find(point[j].ID);
                            editpoint.PointValueID = value;
                            db.SaveChanges();
                        }
                    }
                }
                TeacherPutPointViewModel modelTeacher = new TeacherPutPointViewModel();
                modelTeacher.PointsView = db.Points
                        .Include(c => c.PointLevel)
                        .Include(c => c.PointValue)
                        .Include(c => c.User)
                        .Where(c => c.SubjectID == subjectid)
                        .Select(c => new PointViewModels()
                        {
                            ID = c.ID,
                            SelectedUser = c.User.LastName + " " + c.User.FirstName,
                            UserID = c.UserId,
                            SelectedPointLevel = c.PointLevel.Name,
                            PointLevelID = c.PointLevelID,
                            SelectedPointValue = c.PointValue.Name,
                            PointValueID = c.PointValueID,
                            Date = c.Date
                        })
                        .OrderBy(c => c.PointLevelID)
                        .ThenBy(c => c.Date)
                        .ToList();
                modelTeacher.PointsView.ForEach(c => c.StrDate = c.Date.ToString("dd-MM-yyyy"));
                Guid schoolID = Guid.Parse(db.StudySubject.Where(c => c.SubjectID == subjectid).Select(c => c.User.SchoolID.ToString()).FirstOrDefault());
                modelTeacher.PointValues = db.PointValues
                    .OrderBy(c => c.Value)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                modelTeacher.PointLevels = db.PointLevels
                    .OrderBy(c => c.Level)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                return Json(modelTeacher,JsonRequestBehavior.AllowGet);
            }
        }
        [Roles(Roles.Teacher)]
        public JsonResult AddPoint(UserPointViewModel model)
        {
            using (JournalContext db = new JournalContext())
            {
                if (model.pointLevel == null)
                {
                    return Json(JsonRequestBehavior.DenyGet);
                }
                Guid subjectid = Guid.Parse(model.subjectid);
                for (int i = 0; i < model.userList.Count(); i++)
                {
                    Point point = new Point();
                    point.ID = Guid.NewGuid();
                    point.PointLevelID = Guid.Parse(model.pointLevel);
                    point.SubjectID = Guid.Parse(model.subjectid);
                    point.UserId = Guid.Parse(model.userList[i].user);
                    if (model.userList[i].pointValue == null)
                    {
                        point.PointValueID = null;
                    }
                    else
                    {
                        point.PointValueID = Guid.Parse(model.userList[i].pointValue);
                    }
                    point.Date = DateTime.Now;
                    db.Points.Add(point);
                    db.SaveChanges();
                }
                TeacherPutPointViewModel modelTeacher = new TeacherPutPointViewModel();
                modelTeacher.PointsView = db.Points
                        .Include(c => c.PointLevel)
                        .Include(c => c.PointValue)
                        .Include(c => c.User)
                        .Where(c => c.SubjectID == subjectid)
                        .Select(c => new PointViewModels()
                        {
                            ID = c.ID,
                            SelectedUser = c.User.LastName + " " + c.User.FirstName,
                            UserID = c.UserId,
                            SelectedPointLevel = c.PointLevel.Name,
                            PointLevelID = c.PointLevelID,
                            SelectedPointValue = c.PointValue.Name,
                            PointValueID = c.PointValueID,
                            Date = c.Date
                        })
                        .OrderBy(c => c.PointLevelID)
                        .ThenBy(c => c.Date)
                        .ToList();
                modelTeacher.PointsView.ForEach(c => c.StrDate = c.Date.ToString("dd-MM-yyyy"));
                Guid schoolID = Guid.Parse(db.StudySubject.Where(c => c.SubjectID == subjectid).Select(c => c.User.SchoolID.ToString()).FirstOrDefault());
                modelTeacher.PointValues = db.PointValues
                    .OrderBy(c => c.Value)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                modelTeacher.PointLevels = db.PointLevels
                    .OrderBy(c => c.Level)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();

                return Json(modelTeacher,JsonRequestBehavior.AllowGet);
            }
        }
        [Roles(Roles.Teacher)]
        public JsonResult Edit(Guid subjectid,Guid pointLevelid,string dataEdit)
        {
            using (JournalContext db = new JournalContext())
            {
                TeacherPutPointViewModel modelTeacher = new TeacherPutPointViewModel();
                modelTeacher.PointLevelEdit = pointLevelid;
                modelTeacher.DataEdit = dataEdit;
                modelTeacher.PointsView = db.Points
                        .Include(c => c.PointLevel)
                        .Include(c => c.PointValue)
                        .Include(c => c.User)
                        .Where(c => c.SubjectID == subjectid)
                        .Select(c => new PointViewModels()
                        {
                            ID = c.ID,
                            SelectedUser = c.User.LastName + " " + c.User.FirstName,
                            UserID = c.UserId,
                            SelectedPointLevel = c.PointLevel.Name,
                            PointLevelID = c.PointLevelID,
                            SelectedPointValue = c.PointValue.Name,
                            PointValueID = c.PointValueID,
                            Date = c.Date
                        })
                        .OrderBy(c => c.PointLevelID)
                        .ThenBy(c => c.Date)
                        .ToList();
                modelTeacher.PointsView.ForEach(c => c.StrDate = c.Date.ToString("dd-MM-yyyy"));
                Guid schoolID = Guid.Parse(db.StudySubject.Where(c => c.SubjectID == subjectid).Select(c => c.User.SchoolID.ToString()).FirstOrDefault());
                modelTeacher.PointValues = db.PointValues
                    .OrderBy(c => c.Value)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                modelTeacher.PointLevels = db.PointLevels
                    .OrderBy(c => c.Level)
                    .Where(c => c.SchoolID == schoolID)
                    .Select(c => new SelectListViewModel() { Value = c.ID.ToString(), Text = c.Name }).ToList();
                return Json(modelTeacher,JsonRequestBehavior.AllowGet);
            }
        }


    }
}

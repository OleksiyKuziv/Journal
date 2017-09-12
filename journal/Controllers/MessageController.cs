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
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            using (JournalContext db = new JournalContext())
            {
                MessageViewModels model = new MessageViewModels();
                model.MessageTypeList = db.MessageTypes.Select(c => new SelectListViewModel()
                {
                    Value=c.ID.ToString(),
                    Text=c.Name
                })
                .OrderBy(c=>c.Text)
                .ToList();
                return View(model);
            }
        }
        public JsonResult UserList()
        {
            using (JournalContext db = new JournalContext())
            {
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                       .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid id;
                if (Guid.TryParse(idString, out id))
                {
                    User user = db.Users.Find(id);
                    List<SelectListViewModel> users = db.Users
                        .Include(c => c.UserRole)
                        .Include(c => c.School)
                        .Where(c => c.SchoolID == user.SchoolID&&c.ID!=user.ID)
                        .Select(c => new SelectListViewModel()
                        {
                            Value = c.ID.ToString(),
                            Text = c.FirstName + " " + c.LastName,
                            ValueClass = c.Class.Name,
                            ValueRole = c.UserRole.Name,
                        })
                        .OrderBy(c => c.Text)
                        .ToList();
                    return Json(users, JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult ChatInfo(Guid selectedUser, Guid currentMessangeType)
        {
            using (JournalContext db = new JournalContext())
            {
                var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                       .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid idUser;
                if (Guid.TryParse(idString, out idUser))
                {
                    User user = db.Users.Find(idUser);
                    var listOfMessage = db.Messages
                        .Include(c => c.FromUser)
                        .Include(c => c.ToUser)
                        .Include(c => c.MessageType)
                        .Where(c => (c.ToUserID == selectedUser
                        && c.FromUserID == user.ID
                        && c.MessageTypeID == currentMessangeType)
                        &&
                         (c.ToUserID == user.ID
                        && c.FromUserID == selectedUser
                        && c.MessageTypeID == currentMessangeType))
                        .Select(c => new MessageViewModels()
                        {
                            NameFromUser=c.FromUser.FirstName+" "+c.FromUser.LastName,
                            NameToUser=c.ToUser.FirstName+" "+c.ToUser.LastName,
                            Subject=c.Subject,
                            TimeStamp=c.TimeStamp,
                            Text=c.Text,
                            currentMessageType=c.MessageType.Name            

                        }).ToList();
                    return Json(listOfMessage,JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult AddEmailMessage(MessageViewModels model)
        {
            using (JournalContext db = new JournalContext())
            { var identity = (ClaimsIdentity)User.Identity;
                var idString = identity.Claims
                       .Where(c => c.Type == ClaimTypes.NameIdentifier)
                       .Select(c => c.Value).SingleOrDefault();
                Guid idUser;
                if (Guid.TryParse(idString, out idUser))
                {
                    User user = db.Users.Find(idUser);
                    Message message = new Message();
                    message.ID = Guid.NewGuid();
                    message.FromUserID = user.ID;
                    message.ToUserID = Guid.Parse(model.NameToUser);
                    message.MessageTypeID = Guid.Parse(model.currentMessageType);
                    message.Subject = model.Subject;
                    message.Text = model.Text;
                    message.TimeStamp = DateTime.Now;
                    db.Messages.Add(message);
                    db.SaveChanges();
                    var listOfMessage = db.Messages
                       .Include(c => c.FromUser)
                       .Include(c => c.ToUser)
                       .Include(c => c.MessageType)
                       .Where(c => c.ToUserID == message.ToUserID
                       && c.FromUserID == user.ID
                       && c.MessageTypeID == message.MessageTypeID)
                       .Select(c => new MessageViewModels()
                       {
                           NameFromUser = c.FromUser.FirstName + " " + c.FromUser.LastName,
                           NameToUser = c.ToUser.FirstName + " " + c.ToUser.LastName,
                           Subject = c.Subject,
                           TimeStamp = c.TimeStamp,
                           Text = c.Text,
                           currentMessageType = c.MessageType.Name

                       }).ToList();
                    return Json(listOfMessage,JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.DenyGet);
            }
        }



#region CreateMessageType
        //public ActionResult CreateMessageType()
        //{
        //    MessageType messageType = new MessageType();
        //    messageType.ID = Guid.NewGuid();
        //    messageType.Name = "Email";
        //    using (JournalContext db = new JournalContext())
        //    {
        //        db.MessageTypes.Add(messageType);
        //        db.SaveChanges();

        //    }
        //        return RedirectToAction("Index");

        //}
#endregion
    }
}

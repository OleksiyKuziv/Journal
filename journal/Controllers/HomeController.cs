using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using journal.Models;

namespace journal.Controllers
{
    public class HomeController : Controller
    {
        JournalContext db = new JournalContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Berezhany, school № 3";

            return View();
        }

        [HttpGet]
        public ActionResult Contact() {

            return View();
        }

    [HttpPost]
        public ActionResult Contact(string fname,string lname, string description)
        {
            ViewBag.Message = "Thank you for your description.";

            return View();
        }
    

       }
}
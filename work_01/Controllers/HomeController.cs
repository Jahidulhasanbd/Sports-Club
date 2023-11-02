using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using work_01.Models;

namespace work_01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClubDbContext db = new ClubDbContext();
        public ActionResult Index()
        {
            return View(db.Sports.ToList());
        }
       
    }
}
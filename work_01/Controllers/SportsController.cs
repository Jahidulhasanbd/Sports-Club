using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Schema;
using work_01.Models;
using work_01.Models.ViewModels;

namespace work_01.Controllers
{
    public class SportsController : Controller
    {
        private readonly ClubDbContext db = new ClubDbContext();        
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult MasterView(int page=1,string sort="",string search="")
        {
            ViewBag.NameSort = sort == "name" ? "name_desc" : "name";
            ViewBag.CurrentSort = sort;
            ViewBag.Search = search;
            var data = db.Sports.AsEnumerable().Select(x => new SportsViewModel
            {
                SportsId = x.SportsId,
                SportsName = x.SportsName,
                PlayerCount = x.Players.Count
            }).ToList();
            switch (sort)
            {
                case "name":
                    data = data.OrderBy(x => x.SportsName).ToList();
                    break;
                case "name_desc":
                    data = data.OrderByDescending(x => x.SportsName).ToList();
                    break;
                default:
                    data = data.OrderBy(x => x.SportsName).ToList();
                    break;
            }
            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.SportsName.ToLower().StartsWith(search.ToLower())).ToList();
            }
            var modelData = data.ToPagedList(page, 5);

            return PartialView("_masterDetail",modelData);
        }
        public ActionResult PlayerList(int id)
        {
            return PartialView("_playerList", db.Players.Include("Sports").Where(x => x.SportsId == id).ToList());
        }
        public ActionResult CrudUI(int page=1)
        {
            return View(db.Sports.OrderBy(x=>x.SportsId).ToPagedList(page,5));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="SportsId,SportsName")] Sports sports)
        {
            if (ModelState.IsValid)
            {
                db.Sports.Add(sports);
                db.SaveChanges();
                return RedirectToAction("CrudUI");
            }
            return View(sports);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sports sports = db.Sports.Find(id);
            if (sports==null)
            {
                return HttpNotFound();
            }
            return View(sports);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       public ActionResult Edit([Bind(Include ="SportsId,SportsName")]Sports sports)
        {
            if(ModelState.IsValid)
            {
                db.Entry(sports).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sports);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sports sports = db.Sports.Find(id);
            if (sports == null)
            {
                return HttpNotFound();
            }
            return View(sports);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DoDelete(int id)
        {
            Sports sports = db.Sports.Find(id);
            db.Sports.Remove(sports);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
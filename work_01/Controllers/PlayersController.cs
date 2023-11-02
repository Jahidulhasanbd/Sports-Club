using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using work_01.Models;
using work_01.Models.ViewModels;

namespace work_01.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ClubDbContext db = new ClubDbContext();        
        public ActionResult Index(int page=1)
        {
            var players = db.Players.Include("Sports");

            return View(players.OrderBy(x=>x.PlayerId).ToPagedList(page,5));
        }
        public ActionResult Create()
        {
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlayersViewModel evm)
        {
            if(ModelState.IsValid)
            {
                if(evm.Picture!=null)
                {
                    string filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(evm.Picture.FileName));
                    evm.Picture.SaveAs(Server.MapPath(filePath));

                    Player player = new Player
                    {
                        PlayerName = evm.PlayerName,
                        JoinDate = evm.JoinDate,
                        Grade = evm.Grade,
                        SportsId = evm.SportsId,
                        PicturePath = filePath
                    };
                    db.Players.Add(player);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(evm);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player=db.Players.Find(id);
            if(player == null)
            {
                return HttpNotFound();
            }
            PlayersViewModel evm = new PlayersViewModel
            {
                PlayerId=player.PlayerId,
                PlayerName=player.PlayerName,
                JoinDate=player.JoinDate,
                Grade=player.Grade,
                SportsId=player.SportsId,
                PicturePath=player.PicturePath
            };
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(evm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayersViewModel evm)
        {
            if(ModelState.IsValid)
            {
                string filePath = evm.PicturePath;
                if (evm.Picture != null)
                {
                    filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(evm.Picture.FileName));
                    evm.Picture.SaveAs(Server.MapPath(filePath));

                    Player player = new Player
                    {
                        PlayerId = evm.PlayerId,
                        PlayerName = evm.PlayerName,
                        JoinDate = evm.JoinDate,
                        Grade = evm.Grade,
                        SportsId = evm.SportsId,
                        PicturePath = filePath
                    };
                    db.Entry(player).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Player player = new Player
                    {
                        PlayerId = evm.PlayerId,
                        PlayerName = evm.PlayerName,
                        JoinDate = evm.JoinDate,
                        Grade = evm.Grade,
                        SportsId = evm.SportsId,
                        PicturePath = filePath
                    };
                    db.Entry(player).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.sports = new SelectList(db.Sports, "SportsId", "SportsName");
            return View(evm);
        }
    }
}
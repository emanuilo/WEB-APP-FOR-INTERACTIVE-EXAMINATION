using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class KanalsController : Controller
    {
        private baza db = new baza();

        // GET: Kanals
        public ActionResult Index()
        {
            Korisnik korisnik = getKorisnik();
            var kanals = db.Kanals.Where(p => p.IdKor == korisnik.IdKor).Include(k => k.Korisnik);

            return View(kanals.ToList());
        }

        // GET: Kanals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kanal kanal = db.Kanals.Find(id);
            if (kanal == null)
            {
                return HttpNotFound();
            }
            return View(kanal);
        }

        // GET: Kanals/Create
        public ActionResult Create()
        {
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime");
            return View();
        }

        // POST: Kanals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdKan,Naziv,Lozinka,Otvoren,VrOgranicen,IntervalTrajanja")] Kanal kanal)
        {
            if (ModelState.IsValid)
            {
                Korisnik korisnik = getKorisnik();

                kanal.VrOtvaranja = DateTime.Now;
                kanal.IdKor = korisnik.IdKor;
                if (kanal.Zatvoren == true)
                    kanal.VrZatvaranja = DateTime.Now;

                db.Kanals.Add(kanal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", kanal.IdKor);
            return View(kanal);
        }

        // GET: Kanals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kanal kanal = db.Kanals.Find(id);
            if (kanal == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", kanal.IdKor);
            return View(kanal);
        }

        // POST: Kanals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdKan,Naziv,VrOtvaranja,VrZatvaranja,Lozinka,Otvoren,VrOgranicen,IntervalTrajanja,IdKor")] Kanal kanal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kanal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", kanal.IdKor);
            return View(kanal);
        }

        // GET: Kanals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kanal kanal = db.Kanals.Find(id);
            if (kanal == null)
            {
                return HttpNotFound();
            }
            return View(kanal);
        }

        // POST: Kanals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kanal kanal = db.Kanals.Find(id);
            db.Kanals.Remove(kanal);
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

        public Korisnik getKorisnik()
        {
            if(Session["email"] != null)
            {
                string email = Session["email"].ToString();
                Korisnik korisnik = db.Korisniks.Where(a => a.Email.Equals(email)).FirstOrDefault<Korisnik>();

                return korisnik;
            }
            else
            {
                return null;
            }
        }
    }
}

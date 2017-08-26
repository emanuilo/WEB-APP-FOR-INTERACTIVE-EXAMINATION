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
    public class NarudzbinasController : Controller
    {
        private baza db = new baza();

        // GET: Narudzbinas
        public ActionResult Index()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() == "admin")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            string email = Session["email"].ToString();
            Korisnik korisnik = db.Korisniks.Where(a => a.Email.Equals(email)).FirstOrDefault<Korisnik>();
            int idKor = korisnik.IdKor;

            var narudzbinas = db.Narudzbinas.Include(n => n.Korisnik).Where(a => a.IdKor == idKor);
            return View(narudzbinas.ToList());
        }

        // GET: Narudzbinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = db.Narudzbinas.Find(id);
            if (narudzbina == null)
            {
                return HttpNotFound();
            }
            return View(narudzbina);
        }

        // GET: Narudzbinas/Create
        public ActionResult Create()
        {
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime");
            return View();
        }

        // POST: Narudzbinas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNar,BrTokena,Cena,Status,IdKor")] Narudzbina narudzbina)
        {
            if (ModelState.IsValid)
            {
                db.Narudzbinas.Add(narudzbina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", narudzbina.IdKor);
            return View(narudzbina);
        }

        // GET: Narudzbinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = db.Narudzbinas.Find(id);
            if (narudzbina == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", narudzbina.IdKor);
            return View(narudzbina);
        }

        // POST: Narudzbinas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNar,BrTokena,Cena,Status,IdKor")] Narudzbina narudzbina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(narudzbina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", narudzbina.IdKor);
            return View(narudzbina);
        }

        // GET: Narudzbinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Narudzbina narudzbina = db.Narudzbinas.Find(id);
            if (narudzbina == null)
            {
                return HttpNotFound();
            }
            return View(narudzbina);
        }

        // POST: Narudzbinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Narudzbina narudzbina = db.Narudzbinas.Find(id);
            db.Narudzbinas.Remove(narudzbina);
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

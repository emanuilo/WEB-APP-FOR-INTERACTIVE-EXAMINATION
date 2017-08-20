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
    public class ParametrisController : Controller
    {
        private baza db = new baza();

        // GET: Parametris
        public ActionResult Index()
        {
            return View(db.Parametris.ToList());
        }

        // GET: Parametris/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametri parametri = db.Parametris.Find(id);
            if (parametri == null)
            {
                return HttpNotFound();
            }
            return View(parametri);
        }

        // GET: Parametris/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parametris/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPar,K,M,E,S,G,P")] Parametri parametri)
        {
            if (ModelState.IsValid)
            {
                db.Parametris.Add(parametri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(parametri);
        }

        // GET: Parametris/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "admin")
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametri parametri = db.Parametris.Find(id);
            if (parametri == null)
            {
                return HttpNotFound();
            }
            return View(parametri);
        }

        // POST: Parametris/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPar,K,M,E,S,G,P")] Parametri parametri)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "admin")
            {
                return RedirectToAction("UnauthorizedAccess", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(parametri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(parametri);
        }

        // GET: Parametris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametri parametri = db.Parametris.Find(id);
            if (parametri == null)
            {
                return HttpNotFound();
            }
            return View(parametri);
        }

        // POST: Parametris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parametri parametri = db.Parametris.Find(id);
            db.Parametris.Remove(parametri);
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

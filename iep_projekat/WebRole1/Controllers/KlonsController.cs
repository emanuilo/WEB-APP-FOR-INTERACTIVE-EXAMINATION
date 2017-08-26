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
    public class KlonsController : Controller
    {
        private baza db = new baza();

        public Korisnik getKorisnik()
        {
            if (Session["email"] != null)
            {
                using (baza db = new baza())
                {
                    string email = Session["email"].ToString();
                    Korisnik korisnik = db.Korisniks.Where(a => a.Email.Equals(email)).FirstOrDefault<Korisnik>();

                    return korisnik;
                }
            }
            else
            {
                return null;
            }
        }

        // GET: Klons
        public ActionResult Index()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            var klons = db.Klons.Include(k => k.Kanal).Include(k => k.Pitanje);
            return View(klons.ToList());
        }

        // GET: Klons/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klon klon = db.Klons.Find(id);
            if (klon == null)
            {
                return HttpNotFound();
            }
            ViewBag.K = db.Parametris.FirstOrDefault().K;
            return View(klon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int answer, string IdKlo)
        {
            int idKlo = Convert.ToInt32(IdKlo);

            Korisnik korisnik = getKorisnik();
            Klon klon = db.Klons.Find(idKlo);
            Prati prati = db.Pratis.Where(p => p.IdKor == korisnik.IdKor).Where(p => p.IdKan == klon.IdKan).FirstOrDefault();
            KlonPonudjeniOdg ponudjeni = db.KlonPonudjeniOdgs.Where(p => p.IdKlo == idKlo).Where(p => p.RedniBr == answer).FirstOrDefault();
            Odgovor odgovor = new Odgovor();
            odgovor.IdKlo = idKlo;
            odgovor.IdKor = korisnik.IdKor;
            odgovor.VrSlanja = DateTime.Now;
            odgovor.IdKPO = ponudjeni.IdKPO;
            odgovor.IdKan = (int)klon.IdKan;

            db.Odgovors.Add(odgovor);
            db.SaveChanges();

            if(prati.Evaluacija == true)
            {
                if (ponudjeni.Tacan == true)
                    return RedirectToAction("Correct", new { questionId = klon.IdKlo, answer = answer});
                else
                    return RedirectToAction("Wrong", new { questionId = klon.IdKlo, answer = answer });
            }
            else
            {
                return RedirectToAction("Success");
            }
        }

        // GET: Klons/Create
        public ActionResult Create()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv");
            ViewBag.IdPit = new SelectList(db.Pitanjes, "IdPit", "Naslov");
            return View();
        }

        // POST: Klons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdKlo,Naslov,Tekst,Slika,VrPravljenja,VrPoslZaklj,Zakljucano,IdPit,IdKan")] Klon klon)
        {
            if (ModelState.IsValid)
            {
                db.Klons.Add(klon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv", klon.IdKan);
            ViewBag.IdPit = new SelectList(db.Pitanjes, "IdPit", "Naslov", klon.IdPit);
            return View(klon);
        }

        // GET: Klons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klon klon = db.Klons.Find(id);
            if (klon == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv", klon.IdKan);
            ViewBag.IdPit = new SelectList(db.Pitanjes, "IdPit", "Naslov", klon.IdPit);
            return View(klon);
        }

        // POST: Klons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdKlo,Naslov,Tekst,Slika,VrPravljenja,VrPoslZaklj,Zakljucano,IdPit,IdKan")] Klon klon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(klon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv", klon.IdKan);
            ViewBag.IdPit = new SelectList(db.Pitanjes, "IdPit", "Naslov", klon.IdPit);
            return View(klon);
        }

        // GET: Klons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klon klon = db.Klons.Find(id);
            if (klon == null)
            {
                return HttpNotFound();
            }
            return View(klon);
        }

        // POST: Klons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Klon klon = db.Klons.Find(id);
            db.Klons.Remove(klon);
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

        public ActionResult Success()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            return View();
        }

        public  ActionResult Correct(int questionId, int answer)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            Klon question = db.Klons.Find(questionId);
            ViewBag.K = db.Parametris.FirstOrDefault().K;
            ViewBag.Answer = answer;

            return View(question);
        }

        public ActionResult Wrong(int questionId, int answer)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            Klon question = db.Klons.Find(questionId);
            ViewBag.K = db.Parametris.FirstOrDefault().K;
            ViewBag.Answer = answer;

            return View(question);
        }

        public ActionResult History()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            Korisnik korisnik = getKorisnik();
            ViewBag.IdKor = korisnik.IdKor;
            var clones = db.Klons.SqlQuery("select k.* from Klon k, Odgovor o where k.IdKlo = o.IdKlo and o.IdKor=" + korisnik.IdKor+" order by o.VrSlanja desc").ToList();
            return View(clones);
        }

        public ActionResult HistoryDetails(int id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            Korisnik korisnik = getKorisnik();
            Klon klon = db.Klons.Find(id);
            var odgovor = db.Odgovors.Where(p => p.IdKlo == klon.IdKlo).Where(p => p.IdKor == korisnik.IdKor).FirstOrDefault();

            ViewBag.K = db.Parametris.FirstOrDefault().K;
            ViewBag.Answer = odgovor.KlonPonudjeniOdg.RedniBr;

            return View(klon);
        }
    }

}

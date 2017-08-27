using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebRole1.Hubs;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class KanalsController : Controller
    {
        private baza db = new baza();

        private void CloseChannels()
        {
            List<Kanal> kanals = db.Kanals.Where(p => p.VrOgranicen == true).Where(p => p.Status == "Otvoren").ToList();
            foreach (var kanal in kanals)
            {
                TimeSpan minutesPast = DateTime.Now - kanal.VrOtvaranja;
                if (minutesPast.TotalMinutes > kanal.IntervalTrajanja)
                {
                    kanal.VrZatvaranja = kanal.VrOtvaranja.AddMinutes((double)kanal.IntervalTrajanja);
                    kanal.Status = "Zatvoren";

                    db.Entry(kanal).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        // GET: Kanals
        public ActionResult Index()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();
            Korisnik korisnik = getKorisnik();
            var kanals = db.Kanals.Where(p => p.IdKor == korisnik.IdKor).Include(k => k.Korisnik);

            return View(kanals.ToList());
        }

        // GET: Kanals/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();
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
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime");
            ViewBag.Status = new SelectList(new List<string> { "Na cekanju", "Otvoren", "Zatvoren" });
            return View();
        }

        // POST: Kanals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdKan,Naziv,Lozinka,Status,VrOgranicen,IntervalTrajanja")] Kanal kanal)
        {
            if (ModelState.IsValid)
            {
                Korisnik korisnik = getKorisnik();

                kanal.VrOtvaranja = DateTime.Now;
                kanal.IdKor = korisnik.IdKor;
                if (kanal.Status == "Zatvoren")
                    kanal.VrZatvaranja = DateTime.Now;

                db.Kanals.Add(kanal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(new List<string> { "Na cekanju", "Otvoren", "Zatvoren" });
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", kanal.IdKor);
            return View(kanal);
        }

        // GET: Kanals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();
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
            ViewBag.Status = new SelectList(new List<string> { "Na cekanju", "Otvoren", "Zatvoren" }, kanal.Status);
            return View(kanal);
        }

        // POST: Kanals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdKan,IdKor,Naziv,VrOtvaranja,VrZatvaranja,Lozinka,Status,VrOgranicen,IntervalTrajanja")] Kanal kanal)
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
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();
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
            if (Session["email"] != null)
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

        public ActionResult PublishList(int id)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();

            Korisnik korisnik = getKorisnik();
            var pitanjes = db.Pitanjes.Where(p => p.IdKor == korisnik.IdKor).Where(p => p.Zakljucano == true).Include(p => p.Korisnik);
            Kanal channel = db.Kanals.Find(id);
            ViewBag.ChannelName = channel.Naziv;
            ViewBag.ChannelId = id;
            ViewBag.UserId = korisnik.IdKor;
            return View(pitanjes.ToList());
        }

        [HttpPost, ActionName("PublishList")]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(int id, int? IdPit)
        {
            Pitanje pitanje = db.Pitanjes.Find(IdPit);
            Kanal kanal = db.Kanals.Find(id);
            Klon klon = new Klon();

            klon.IdPit = pitanje.IdPit;
            klon.Naslov = pitanje.Naslov;
            klon.Tekst = pitanje.Tekst;
            klon.Slika = pitanje.Slika;
            klon.ImageToUpload = pitanje.ImageToUpload;
            klon.VrPravljenja = pitanje.VrPravljenja;
            klon.VrPoslZaklj = pitanje.VrPoslZaklj;
            klon.Zakljucano = pitanje.Zakljucano;
            klon.IdKan = id;

            db.Klons.Add(klon);
            db.SaveChanges();

            var ponudjeniOdgs = db.PonudjeniOdgs.Where(p => p.IdPit == pitanje.IdPit).ToList();
            string stringPonudjeni = "";
            foreach (var ponudjeni in ponudjeniOdgs)
            {
                KlonPonudjeniOdg klonPonudjeni = new KlonPonudjeniOdg();
                klonPonudjeni.Sadrzaj = ponudjeni.Sadrzaj;
                klonPonudjeni.RedniBr = ponudjeni.RedniBr;
                klonPonudjeni.Tacan = ponudjeni.Tacan;
                klonPonudjeni.IdKlo = klon.IdKlo;
                stringPonudjeni += klonPonudjeni.Sadrzaj + ";";
                db.KlonPonudjeniOdgs.Add(klonPonudjeni);
            }

            db.SaveChanges();

            //SignalR
            var context = GlobalHost.ConnectionManager.GetHubContext<PushHub>();
            context.Clients.Group("Channel " + id).addNewQuestion(klon.Naslov, klon.Tekst, stringPonudjeni, klon.IdKlo, kanal.Naziv);

            return RedirectToAction("PublishList");
        }

        public ActionResult PublishedQuestions(int idKan)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();

            var klones = db.Klons.Where(p => p.IdKan == idKan);
            ViewBag.IdKan = idKan;
            return View(klones.ToList());
        }

        public ActionResult ChannelsStudent()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            CloseChannels();

            var kanals = db.Kanals.Where(p => p.Status == "Otvoren");
            ViewBag.IdKor = getKorisnik().IdKor;
            return View(kanals.ToList());
        }

        public ActionResult Subscribe(int idKan)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "student")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            Parametri parametri = db.Parametris.FirstOrDefault<Parametri>();
            ViewBag.E = parametri.E;
            ViewBag.ChannelName = db.Kanals.Find(idKan).Naziv;
            return View();
        }

        [HttpPost, ActionName("Subscribe")]
        [ValidateAntiForgeryToken]
        public ActionResult SubsribeConfirmation(int idKan, string Lozinka, bool Evaluation)
        {
            Kanal kanal = db.Kanals.Find(idKan);
            Parametri parametri = db.Parametris.FirstOrDefault<Parametri>();
            if (Lozinka != kanal.Lozinka)
            {
                ModelState.AddModelError(string.Empty, "Pogresna lozinka!");
                ViewBag.E = parametri.E;
                ViewBag.ChannelName = kanal.Naziv;
                return View();
            }

            Korisnik korisnik = getKorisnik();
            if (Evaluation == true && korisnik.BrTokena < parametri.E)
            {
                ModelState.AddModelError(string.Empty, "Nedovoljno tokena!");
                ViewBag.E = parametri.E;
                ViewBag.ChannelName = kanal.Naziv;
                return View();
            }

            Prati prati = new Prati();
            prati.IdKan = idKan;
            prati.IdKor = korisnik.IdKor;
            prati.Evaluacija = Evaluation;

            db.Pratis.Add(prati);
            db.SaveChanges();

            return RedirectToAction("ChannelsStudent");
        }

        public ActionResult Answers(int IdKlo)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() != "profesor")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            var answers = db.Odgovors.Where(p => p.IdKlo == IdKlo).Include(p => p.Korisnik).Include(p => p.KlonPonudjeniOdg).ToList();
            return View(answers);
        }
    }
}

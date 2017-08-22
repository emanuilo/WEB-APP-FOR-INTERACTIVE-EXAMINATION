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
    public class PitanjesController : Controller
    {
        private baza db = new baza();

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

        // GET: Pitanjes
        public ActionResult Index()
        {
            Korisnik korisnik = getKorisnik();
            var pitanjes = db.Pitanjes.Where(p => p.IdKor == korisnik.IdKor).Include(p => p.Kanal).Include(p => p.Korisnik);
            return View(pitanjes.ToList());
        }

        // GET: Pitanjes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pitanje pitanje = db.Pitanjes.Find(id);
            if (pitanje == null)
            {
                return HttpNotFound();
            }
            ViewBag.K = (int)db.Parametris.FirstOrDefault<Parametri>().K;
            return View(pitanje);
        }

        // GET: Pitanjes/Create
        public ActionResult Create()
        {
            Parametri parametri = db.Parametris.FirstOrDefault<Parametri>();
            ViewBag.K = parametri.K;
            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv");
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime");
            return View();
        }

        // POST: Pitanjes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pitanje pitanje)
        {
            if (ModelState.IsValid)
            {
                Korisnik korisnik = getKorisnik();
                int K = (int)db.Parametris.FirstOrDefault<Parametri>().K;

                // Convert HttpPostedFileBase to byte array.
                if(pitanje.ImageToUpload != null)
                {
                    pitanje.Slika = new byte[pitanje.ImageToUpload.ContentLength];
                    pitanje.ImageToUpload.InputStream.Read(pitanje.Slika, 0, pitanje.Slika.Length);
                }
                pitanje.VrPravljenja = DateTime.Now;
                pitanje.IdKor = korisnik.IdKor;
                if (pitanje.Zakljucano == true)
                    pitanje.VrPoslZaklj = DateTime.Now;

                int tacanOdgovor = Convert.ToInt32(Request["radioPonudjeno"]);
                for(int i = 0; i < K; i++)
                {
                    string tekstPonudjenog = Request["ponudjeno" + i].ToString();
                    PonudjeniOdg ponudjeni = new PonudjeniOdg();
                    ponudjeni.IdPit = pitanje.IdPit;
                    ponudjeni.Sadrzaj = tekstPonudjenog;
                    ponudjeni.RedniBr = i;
                    ponudjeni.Tacan = (i == tacanOdgovor ? true : false);
                    
                    db.PonudjeniOdgs.Add(ponudjeni);
                }

                db.Pitanjes.Add(pitanje);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv", pitanje.IdKan);
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", pitanje.IdKor);
            return View(pitanje);
        }

        // GET: Pitanjes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pitanje pitanje = db.Pitanjes.Find(id);
            if (pitanje == null)
            {
                return HttpNotFound();
            }
            if (pitanje.Zakljucano == true)
            {
                return RedirectToAction("UnlockQuestion", new { id = id });
            }
            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv", pitanje.IdKan);
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", pitanje.IdKor);
            //TODO edit ponudjenih odgovora
            return View(pitanje);
        }

        // POST: Pitanjes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPit,IdKor,Naslov,Tekst,VrPravljenja,VrPoslZaklj,Zakljucano")] Pitanje pitanje)
        {
            if (ModelState.IsValid)
            {
                if(pitanje.Zakljucano == true)
                {
                    pitanje.VrPoslZaklj = DateTime.Now;
                }
                db.Entry(pitanje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKan = new SelectList(db.Kanals, "IdKan", "Naziv", pitanje.IdKan);
            ViewBag.IdKor = new SelectList(db.Korisniks, "IdKor", "Ime", pitanje.IdKor);
            return View(pitanje);
        }

        // GET: Pitanjes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pitanje pitanje = db.Pitanjes.Find(id);
            if (pitanje == null)
            {
                return HttpNotFound();
            }
            return View(pitanje);
        }

        // POST: Pitanjes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pitanje pitanje = db.Pitanjes.Find(id);
            db.Pitanjes.Remove(pitanje);
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

        public ActionResult UnlockQuestion(int id)
        {
            Parametri parametri = db.Parametris.FirstOrDefault<Parametri>();
            Pitanje pitanje = db.Pitanjes.Find(id);
            ViewBag.M = parametri.M;
            return View(pitanje);
        }

        [HttpPost, ActionName("UnlockQuestion")]
        [ValidateAntiForgeryToken]
        public ActionResult UnlockConfirmation(int id)
        {
            Korisnik korisnik = getKorisnik();
            Parametri parametri = db.Parametris.FirstOrDefault<Parametri>();
            Pitanje pitanje = db.Pitanjes.Find(id);

            if (korisnik.BrTokena < parametri.M)
            {
                return RedirectToAction("NotEnoughTokens");
            }

            korisnik.BrTokena -= (int)parametri.M;
            korisnik.PotvrdaLozinke = korisnik.Lozinka;
            pitanje.Zakljucano = false;

            db.Entry(pitanje).State = EntityState.Modified;
            db.Entry(korisnik).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult NotEnoughTokens()
        {
            return View();
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole1.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Net.Mail;

namespace WebRole1.Controllers
{
    public class HomeController : Controller
    {
        private baza db1 = new baza();

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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Lozinka")] Korisnik korisnik)
        {
           using (baza db = new baza())
            {
                //var query = db.Korisniks.Where(a => a.Email.Equals(korisnik.Email)).Where(a => a.Lozinka.Equals(korisnik.Lozinka));
                var query = db.Korisniks.Where(a => a.Email.Equals(korisnik.Email));
                var user = query.FirstOrDefault<Korisnik>();

                if(user != null)
                {
                    PasswordHasher hasher = new PasswordHasher();
                    if(user.Status.ToString() == "neaktivan")
                    {
                        ModelState.AddModelError(string.Empty, "Nalog nije aktiviran");
                    }
                    else if(hasher.VerifyHashedPassword(user.Lozinka, korisnik.Lozinka) == PasswordVerificationResult.Success)
                    {
                        Session["email"] = user.Email.ToString();
                        Session["ime"] = user.Ime.ToString();
                        Session["uloga"] = user.Uloga.ToString();
                        return RedirectToAction("index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Pogresna lozinka");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Pogresan e-mail");
                }
            }

            return View(korisnik);
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                using (baza db = new baza())
                {
                    try
                    {
                        PasswordHasher hasher = new PasswordHasher();
                        korisnik.Lozinka = hasher.HashPassword(korisnik.Lozinka);
                        korisnik.PotvrdaLozinke = korisnik.Lozinka;
                        korisnik.Status = "neaktivan";
                        db.Entry(korisnik).State = System.Data.Entity.EntityState.Modified;

                        db.Korisniks.Add(korisnik);
                        db.SaveChanges();

                        return RedirectToAction("Login");

                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }

                }
            }

            return View(korisnik);
        }

        public ActionResult Logout()
        {
            Session["email"] = null;
            Session["ime"] = null;
            Session["uloga"] = null;

            return RedirectToAction("index");
        }

        public ActionResult AccountDetails()
        {
            if(Session["ime"] == null)
            {
                return RedirectToAction("UnauthorizedAccess");
            }
            using (baza db = new baza())
            {
                string email = Session["email"].ToString(); 
                var query = db.Korisniks.Where(a => a.Email.Equals(email));
                var korisnik = query.FirstOrDefault<Korisnik>();
                return View(korisnik);
            }
        }

        public ActionResult Registrations(string filter)
        {
            if(Session["uloga"] == null || Session["uloga"].ToString() != "admin")
            {
                return RedirectToAction("UnauthorizedAccess");
            }
            using(baza db = new baza())
            {
                var filterList = new List<string>();
                filterList.Add("aktivan");
                filterList.Add("neaktivan");

                ViewBag.filter = new SelectList(filterList);

                var korisniks = db.Korisniks.ToList();

                if (!string.IsNullOrEmpty(filter))
                {
                    korisniks = korisniks.Where(x => x.Status == filter).ToList();
                }
                return View(korisniks);
            }
        }

        public ActionResult Parameters()
        {
            return RedirectToAction("Edit", "Parametris", new { id = 1 });
        }

        public ActionResult Tokens()
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() == "admin")
            {
                return RedirectToAction("UnauthorizedAccess");
            }
            using (baza db = new baza())
            {
                Parametri parametri = db.Parametris.FirstOrDefault<Parametri>();
                string email = Session["email"].ToString();
                Korisnik korisnik = db.Korisniks.Where(a => a.Email.Equals(email)).FirstOrDefault<Korisnik>();

                ViewBag.tokens = korisnik.BrTokena;
                ViewBag.silver = parametri.S;
                ViewBag.gold = parametri.G;
                ViewBag.platin = parametri.P;
                ViewBag.link = "http://stage.centili.com/payment/widget?apikey=7a6ac8db85d69ee617967f6b11548879";

                return View();
            }
        }

        public ActionResult Order(int numOfTokens)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() == "admin")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            using (baza db = new baza())
            {
                string email = Session["email"].ToString();
                Korisnik korisnik = db.Korisniks.Where(a => a.Email.Equals(email)).FirstOrDefault<Korisnik>();

                Narudzbina narudzbina = new Narudzbina();
                narudzbina.IdKor = korisnik.IdKor;
                narudzbina.Korisnik = korisnik;
                narudzbina.Status = "cekanje na obradu";
                narudzbina.Cena = 50;
                narudzbina.BrTokena = numOfTokens;
                db.Narudzbinas.Add(narudzbina);
                db.SaveChanges();

                return Redirect("http://stage.centili.com/payment/widget?apikey=7a6ac8db85d69ee617967f6b11548879&returnurl=localhost:52051/Home/CentiliReturn/");
            }
        }

        public ActionResult CentiliReturn(string status)
        {
            if (Session["uloga"] == null || Session["uloga"].ToString() == "admin")
            {
                return RedirectToAction("UnauthorizedAccess");
            }

            using (baza db = new baza())
            {
                string email = Session["email"].ToString();
                Korisnik korisnik = db.Korisniks.Where(a => a.Email == email).FirstOrDefault<Korisnik>();
                Narudzbina narudzbina = db.Narudzbinas.Where(a => a.IdKor == korisnik.IdKor).OrderByDescending(x => x.IdNar).FirstOrDefault();

                if (status == "success")
                {

                    narudzbina.Status = "realizovana";
                    db.Entry(narudzbina).State = System.Data.Entity.EntityState.Modified;

                    korisnik.BrTokena = korisnik.BrTokena + narudzbina.BrTokena;
                    korisnik.PotvrdaLozinke = korisnik.Lozinka;
                    db.Entry(korisnik).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    SendEmail();
                    return RedirectToAction("Tokens");
                }
                else
                {
                    narudzbina.Status = "ponistena";
                    db.Entry(narudzbina).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("FailedPurchase");
                }
            }
        }

        public void SendEmail()
        {
            if(Session["email"] == null) { return; }

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("emanuilo.jovanovic@gmail.com", "Emanuilo Jovanovic");
            mail.To.Add(Session["email"].ToString());
            mail.Subject = "Tokeni";
            mail.Body = "Uspesno ste obavili kupovinu tokena!";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("emanuilo.jovanovic@gmail.com", "40razbojnika");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public ActionResult UnauthorizedAccess()
        {
           return View();
        }

        public ActionResult FailedPurchase()
        {
            return View();
        }

        public ActionResult Questions()
        {
            return RedirectToAction("Index", "Pitanjes");
        }

        public ActionResult Channels()
        {
            return RedirectToAction("Index", "Kanals");
        }

        public ActionResult Blackboard()
        {
            //TODO da se vidi iz kog kanala je pitanje
            Korisnik korisnik = getKorisnik();
            Parametri parametri = db1.Parametris.FirstOrDefault<Parametri>();
            ViewBag.K = parametri.K;
            ViewBag.UserId = korisnik.IdKor;
            var klones = db1.Klons.SqlQuery("select kl.* from Klon kl, Kanal ka, Prati p where kl.IdKan = ka.IdKan and p.IdKan = ka.IdKan and p.IdKor =" + korisnik.IdKor + " and kl.IdKlo not in (select IdKlo from Odgovor where IdKor =" + korisnik.IdKor + ")").ToList();
            
            return View(klones);
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
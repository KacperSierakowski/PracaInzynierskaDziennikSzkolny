using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using DziennikSzkolny13.Models;
using Newtonsoft.Json;

namespace DziennikSzkolny13.Controllers
{
    [Authorize]
    public class KlasasController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Klasas
        public ActionResult Index(string SearchBy, string searching)
        {
            if (User.IsInRole("Nauczyciel") || User.IsInRole("Administrator"))
            {
                if (SearchBy == "NazwaKlasy")
                {
                    //var iloscOcenWystawionychPrzezKonkretnegoNauczycicela = db.Klasas.UczniowieKlasy
                    //    .Where(d => d.OcenyUcznia.Any(df => df.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel))).Count();

                    return View(db.Klasas.Include(x => x.UczniowieKlasy).Where(x => x.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else
                {
                    return View(db.Klasas.Include(x => x.UczniowieKlasy).Where(x => x.ProfilKlasy.Contains(searching) || searching == null).ToList());
                }
            }
            /*
            else if (User.IsInRole("Uczeń"))
            {
                //var viewModel = new DziennikSzkolny13DB();
                // viewModel.Klasas = db.Klasas.Where(i=>i.UczniowieKlasy.Where(i=>i.Email.Equals("")));

                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                //var Zalogowany = db.Uczens.Where(i => i.Email.Equals(ZalogowanyUczen));
                // var KlasaZalogowanego = Zalogowany.Where(w => w.klasaUcznia.NazwaKlasy.ToString());

                return View(db.Klasas.Include(u => u.UczniowieKlasy).Where(x => x.UczniowieKlasy.Any(u => u.Email.Equals(ZalogowanyUczen))).ToList());
            }
            */
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // GET: Klasas/Details/5
        public ActionResult Details(int? id)
        {

            string ZalogowanyUser = Request.ServerVariables["LOGON_USER"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klasa klasa = db.Klasas.Find(id);
            if (klasa == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Nauczyciel"))
            {
                int IloscWszystkichOcenWystawionychPrzezNauczyciela = klasa.UczniowieKlasy                        .Where(s => s.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie1 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(1))).Where(p => p.OcenyUcznia.Any(s => s.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie2 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(2))).Where(p => p.OcenyUcznia.Any(s => s.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie3 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(3))).Where(p => p.OcenyUcznia.Any(s => s.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie4 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(4))).Where(p => p.OcenyUcznia.Any(s => s.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie5 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(5))).Where(p => p.OcenyUcznia.Any(s => s.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie6 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(6))).Where(p => p.OcenyUcznia.Any(s => s.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                float Srednia = ((1 * Wszystkie1) + (2 * Wszystkie2) + (3 * Wszystkie3) + (4 * Wszystkie4) + (5 * Wszystkie5) + (6 * Wszystkie6))
                    / IloscWszystkichOcenWystawionychPrzezNauczyciela;
                ViewBag.wszystkie = IloscWszystkichOcenWystawionychPrzezNauczyciela;
                ViewBag.wszystkie1 = Wszystkie1;
                ViewBag.wszystkie2 = Wszystkie2;
                ViewBag.wszystkie3 = Wszystkie3;
                ViewBag.wszystkie4 = Wszystkie4;
                ViewBag.wszystkie5 = Wszystkie5;
                ViewBag.wszystkie6 = Wszystkie6;
                ViewBag.srednia = Srednia;
                return View(klasa);
            }
            else if (User.IsInRole("Administrator"))
            {
                int IloscWszystkichOcenWystawionychPrzezNauczycieli = klasa.UczniowieKlasy.Count();
                int Wszystkie1 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(1))).Count();
                int Wszystkie2 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(2))).Count();
                int Wszystkie3 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(3))).Count();
                int Wszystkie4 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(4))).Count();
                int Wszystkie5 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(5))).Count();
                int Wszystkie6 = klasa.UczniowieKlasy.Where(d => d.OcenyUcznia.Any(q => q.WartoscOceny.Equals(6))).Count();
                float Srednia = (1 * Wszystkie1 + 2 * Wszystkie2 + 3 * Wszystkie3 + 4 * Wszystkie4 + 5 * Wszystkie5 + 6 * Wszystkie6) / IloscWszystkichOcenWystawionychPrzezNauczycieli;
                ViewBag.wszystkie = IloscWszystkichOcenWystawionychPrzezNauczycieli;
                ViewBag.wszystkie1 = Wszystkie1;
                ViewBag.wszystkie2 = Wszystkie2;
                ViewBag.wszystkie3 = Wszystkie3;
                ViewBag.wszystkie4 = Wszystkie4;
                ViewBag.wszystkie5 = Wszystkie5;
                ViewBag.wszystkie6 = Wszystkie6;
                ViewBag.srednia = Srednia;
                return View(klasa);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        [Authorize(Roles = "Administrator")]
        // GET: Klasas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Klasas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NazwaKlasy,ProfilKlasy")] Klasa klasa)
        {
            if (ModelState.IsValid)
            {
                db.Klasas.Add(klasa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(klasa);
        }

        // GET: Klasas/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klasa klasa = db.Klasas.Find(id);
            if (klasa == null)
            {
                return HttpNotFound();
            }
            return View(klasa);
        }

        // POST: Klasas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "ID,NazwaKlasy,ProfilKlasy")] Klasa klasa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(klasa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(klasa);
        }

        // GET: Klasas/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klasa klasa = db.Klasas.Find(id);
            if (klasa == null)
            {
                return HttpNotFound();
            }
            return View(klasa);
        }

        // POST: Klasas/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Klasa klasa = db.Klasas.Find(id);
            db.Klasas.Remove(klasa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public JsonResult IndexForMobileApp()
        //{
        //    var ListaKlas = db.Klasas.Include(x => x.UczniowieKlasy).ToList();
        //    return Json(ListaKlas, JsonRequestBehavior.AllowGet);
        //}

        //public IEnumerable<Klasa> Get()
        //{
        //    return db.Klasas.Include(x => x.UczniowieKlasy).ToList();
        //}

        // GET: Klasas/CreateOcenasFromKlasas
        public ActionResult CreateOcenasFromKlasas(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uczen uczen = db.Uczens.Find(id);
            if (uczen == null)
            {
                return HttpNotFound();
            }
            if ((User.IsInRole("Nauczyciel")))
            {
                string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(u => u.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", uczen.ID);
                return View();
            }
            else if (User.IsInRole("Administrator"))
            {
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", uczen.ID);
                return View();
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // POST: Klasas/CreateOcenasFromKlasas
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOcenasFromKlasas([Bind(Include = "ID,WartoscOceny,UczenID,PrzedmiotID")] Ocena ocena)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Ocenas.Add(ocena);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (User.IsInRole("Nauczyciel"))
                {
                    string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];
                    ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(u => u.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel)), "ID", "NazwaPrzedmiotu", ocena.PrzedmiotID);
                    ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", ocena.UczenID);
                    return View(ocena);
                }
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu", ocena.PrzedmiotID);
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", ocena.UczenID);
                return View(ocena);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // GET: Klasa/CreateNieobecnoscsFromKlasas
        public ActionResult CreateNieobecnoscsFromKlasas(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uczen uczen = db.Uczens.Find(id);
            if (uczen == null)
            {
                return HttpNotFound();
            }
            if ((User.IsInRole("Nauczyciel")))
            {
                string Zalogowany = Request.ServerVariables["LOGON_USER"];
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(s => s.przedmiotNauczyciel.Email.Equals(Zalogowany)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", uczen.ID);
                return View();
            }
            else if (User.IsInRole("Administrator"))
            {
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", uczen.ID);
                return View();
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }


        // POST: Klasas/CreateNieobecnoscsFromKlasas
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNieobecnoscsFromKlasas([Bind(Include = "ID,Data,UczenID,PrzedmiotID,CzyUsprawiedliwiona")] Nieobecnosc nieobecnosc)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Nieobecnoscs.Add(nieobecnosc);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (User.IsInRole("Nauczyciel"))
                {
                    string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];

                    ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(s => s.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel)), "ID", "NazwaPrzedmiotu");
                    ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", nieobecnosc.UczenID);
                    return View(nieobecnosc);
                }
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa", nieobecnosc.UczenID);
                return View(nieobecnosc);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
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

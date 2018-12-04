using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DziennikSzkolny13.Models;

namespace DziennikSzkolny13.Controllers
{
    [Authorize]
    public class PrzedmiotsController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Przedmiots
        public ActionResult Index(string searching)
        {
            if (User.IsInRole("Administrator"))
            {
                var przedmiots = db.Przedmiots.Include(p => p.przedmiotNauczyciel);
                return View(przedmiots.Where(x => x.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
            }
            else if (User.IsInRole("Nauczyciel"))
            {
                string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];
                var przedmiots = db.Przedmiots.Include(p => p.przedmiotNauczyciel);
                return View(przedmiots.Where(u => u.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel)).ToList());
            }
            else if (User.IsInRole("Uczeń"))
            {
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                var przedmiots = db.Przedmiots.Include(p => p.przedmiotNauczyciel).Include(p => p.OcenyWystawionePrzezNauczyciela);
                return View(przedmiots.Where(u => u.OcenyWystawionePrzezNauczyciela.Any(x => x.OcenaUczen.Email.Equals(ZalogowanyUczen))).ToList());
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }
        [Authorize]
        // GET: Przedmiots/Details/5
        public ActionResult Details(int? id)
        {
            string ZalogowanyUser = Request.ServerVariables["LOGON_USER"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przedmiot przedmiot = db.Przedmiots.Find(id);
            if (przedmiot == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Nauczyciel"))
            {
                int IloscWszystkichOcenWystawionychPrzezNauczyciela = przedmiot.OcenyWystawionePrzezNauczyciela.Count();//.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Count();
                int Wszystkie1 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Where(x => x.WartoscOceny.Equals(1)).Count();
                int Wszystkie2 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Where(x => x.WartoscOceny.Equals(2)).Count();
                int Wszystkie3 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Where(x => x.WartoscOceny.Equals(3)).Count();
                int Wszystkie4 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Where(x => x.WartoscOceny.Equals(4)).Count();
                int Wszystkie5 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Where(x => x.WartoscOceny.Equals(5)).Count();
                int Wszystkie6 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.OcenyUcznia.Any(f => f.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyUser))).Where(x => x.WartoscOceny.Equals(6)).Count();
                float Srednia = (1 * Wszystkie1 + 2 * Wszystkie2 + 3 * Wszystkie3 + 4 * Wszystkie4 + 5 * Wszystkie5 + 6 * Wszystkie6) / IloscWszystkichOcenWystawionychPrzezNauczyciela;
                ViewBag.wszystkie = IloscWszystkichOcenWystawionychPrzezNauczyciela;
                ViewBag.wszystkie1 = Wszystkie1;
                ViewBag.wszystkie2 = Wszystkie2;
                ViewBag.wszystkie3 = Wszystkie3;
                ViewBag.wszystkie4 = Wszystkie4;
                ViewBag.wszystkie5 = Wszystkie5;
                ViewBag.wszystkie6 = Wszystkie6;
                ViewBag.srednia = Srednia;

                return View(przedmiot);
            }
            else if (User.IsInRole("Administrator"))
            {
                int IloscWszystkichOcenWystawionychPrzezNauczyciela = przedmiot.OcenyWystawionePrzezNauczyciela.Count();
                int Wszystkie1 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(x => x.WartoscOceny.Equals(1)).Count();
                int Wszystkie2 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(x => x.WartoscOceny.Equals(2)).Count();
                int Wszystkie3 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(x => x.WartoscOceny.Equals(3)).Count();
                int Wszystkie4 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(x => x.WartoscOceny.Equals(4)).Count();
                int Wszystkie5 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(x => x.WartoscOceny.Equals(5)).Count();
                int Wszystkie6 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(x => x.WartoscOceny.Equals(6)).Count();
                float Srednia = (1 * Wszystkie1 + 2 * Wszystkie2 + 3 * Wszystkie3 + 4 * Wszystkie4 + 5 * Wszystkie5 + 6 * Wszystkie6) / IloscWszystkichOcenWystawionychPrzezNauczyciela;
                ViewBag.wszystkie = IloscWszystkichOcenWystawionychPrzezNauczyciela;
                ViewBag.wszystkie1 = Wszystkie1;
                ViewBag.wszystkie2 = Wszystkie2;
                ViewBag.wszystkie3 = Wszystkie3;
                ViewBag.wszystkie4 = Wszystkie4;
                ViewBag.wszystkie5 = Wszystkie5;
                ViewBag.wszystkie6 = Wszystkie6;
                ViewBag.srednia = Srednia;

                return View(przedmiot);
            }
            else
            {
                if (User.IsInRole("Uczeń"))
                {
                    int IloscWzystkichOcen = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Count();
                    int Wszystkie1 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Where(f => f.WartoscOceny.Equals(1)).Count();
                    int Wszystkie2 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Where(f => f.WartoscOceny.Equals(2)).Count();
                    int Wszystkie3 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Where(f => f.WartoscOceny.Equals(3)).Count();
                    int Wszystkie4 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Where(f => f.WartoscOceny.Equals(4)).Count();
                    int Wszystkie5 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Where(f => f.WartoscOceny.Equals(5)).Count();
                    int Wszystkie6 = przedmiot.OcenyWystawionePrzezNauczyciela.Where(d => d.OcenaUczen.Email.Equals(ZalogowanyUser)).Where(f => f.WartoscOceny.Equals(6)).Count();
                    float Srednia = (1 * Wszystkie1 + 2 * Wszystkie2 + 3 * Wszystkie3 + 4 * Wszystkie4 + 5 * Wszystkie5 + 6 * Wszystkie6) / IloscWzystkichOcen;
                    ViewBag.wszystkie = IloscWzystkichOcen;
                    ViewBag.wszystkie1 = Wszystkie1;
                    ViewBag.wszystkie2 = Wszystkie2;
                    ViewBag.wszystkie3 = Wszystkie3;
                    ViewBag.wszystkie4 = Wszystkie4;
                    ViewBag.wszystkie5 = Wszystkie5;
                    ViewBag.wszystkie6 = Wszystkie6;
                    ViewBag.srednia = Srednia;

                    return View(przedmiot);
                }
                else
                {
                    return View("~/Views/Uczens/PermissionDenied.cshtml");
                }
            }
          
        }

        [Authorize(Roles = "Administrator")]
        // GET: Przedmiots/Create
        public ActionResult Create()
        {
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "PelnaNazwa");
            return View();
        }

        // POST: Przedmiots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NazwaPrzedmiotu,NauczycielID")] Przedmiot przedmiot)
        {
            if (ModelState.IsValid)
            {
                db.Przedmiots.Add(przedmiot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "PelnaNazwa", przedmiot.NauczycielID);
            return View(przedmiot);
        }

        // GET: Przedmiots/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przedmiot przedmiot = db.Przedmiots.Find(id);
            if (przedmiot == null)
            {
                return HttpNotFound();
            }
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "PelnaNazwa", przedmiot.NauczycielID);
            return View(przedmiot);
        }

        // POST: Przedmiots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NazwaPrzedmiotu,NauczycielID")] Przedmiot przedmiot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(przedmiot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "PelnaNazwa", przedmiot.NauczycielID);
            return View(przedmiot);
        }

        // GET: Przedmiots/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przedmiot przedmiot = db.Przedmiots.Find(id);
            if (przedmiot == null)
            {
                return HttpNotFound();
            }
            return View(przedmiot);
        }

        // POST: Przedmiots/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Przedmiot przedmiot = db.Przedmiots.Find(id);
            db.Przedmiots.Remove(przedmiot);
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

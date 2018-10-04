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
                return View(przedmiots.Where(u=>u.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel)).ToList());
            }
            else if (User.IsInRole("Uczeń"))
            {
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                var przedmiots = db.Przedmiots.Include(p => p.przedmiotNauczyciel).Include(p=>p.OcenyWystawionePrzezNauczyciela);
                return View(przedmiots.Where(u => u.OcenyWystawionePrzezNauczyciela.Any(x=>x.OcenaUczen.Email.Equals(ZalogowanyUczen))).ToList());
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

        [Authorize(Roles ="Administrator")]
        // GET: Przedmiots/Create
        public ActionResult Create()
        {
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie");
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

            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie", przedmiot.NauczycielID);
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
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie", przedmiot.NauczycielID);
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
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie", przedmiot.NauczycielID);
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

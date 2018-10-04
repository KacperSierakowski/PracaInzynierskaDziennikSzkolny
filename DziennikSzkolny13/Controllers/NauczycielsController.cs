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
    public class NauczycielsController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Nauczyciels
        public ActionResult Index(string searching)
        {
            if (User.IsInRole("Nauczyciel"))
            {
                string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];
                return View(db.Nauczyciels.Include(u => u.PrzedmiotyNauczyciela).Where(x => x.Email.Equals(ZalogowanyNauczyciel)).ToList());
            }
            else if (User.IsInRole("Administrator"))
            {
                return View(db.Nauczyciels.Include(u => u.PrzedmiotyNauczyciela).Where(x => x.Nazwisko.Contains(searching) || searching == null).ToList());
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // GET: Nauczyciels/Details/5
        public ActionResult Details(int? id)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Nauczyciel nauczyciel = db.Nauczyciels.Find(id);
                if (nauczyciel == null)
                {
                    return HttpNotFound();
                }
                return View(nauczyciel);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // GET: Nauczyciels/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nauczyciels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Imie,Nazwisko,NumerTelefonu,Adres,Email")] Nauczyciel nauczyciel)
        {
            if (ModelState.IsValid)
            {
                db.Nauczyciels.Add(nauczyciel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nauczyciel);
        }

        // GET: Nauczyciels/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nauczyciel nauczyciel = db.Nauczyciels.Find(id);
            if (nauczyciel == null)
            {
                return HttpNotFound();
            }
            return View(nauczyciel);
        }

        // POST: Nauczyciels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Imie,Nazwisko,NumerTelefonu,Adres,Email")] Nauczyciel nauczyciel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nauczyciel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nauczyciel);
        }

        // GET: Nauczyciels/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nauczyciel nauczyciel = db.Nauczyciels.Find(id);
            if (nauczyciel == null)
            {
                return HttpNotFound();
            }
            return View(nauczyciel);
        }

        // POST: Nauczyciels/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nauczyciel nauczyciel = db.Nauczyciels.Find(id);
            db.Nauczyciels.Remove(nauczyciel);
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

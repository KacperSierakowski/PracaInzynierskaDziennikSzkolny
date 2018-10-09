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
    public class UczensController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Uczens
        public ActionResult Index(string SearchBy, string searching)
        {
            if (User.IsInRole("Nauczyciel") || User.IsInRole("Administrator"))
            {
                if (SearchBy == "Imie")
                {
                    //ViewBag.Message = "Jestem Super Nauczyciel";
                    var uczens = db.Uczens.Include(u => u.klasaUcznia).Where(x => x.Imie.Contains(searching) || searching == null);
                    return View(uczens.ToList());
                }
                else
                {
                    //ViewBag.Message = "Jestem Super Nauczyciel";
                    var uczens = db.Uczens.Include(u => u.klasaUcznia).Where(x => x.Nazwisko.Contains(searching) || searching == null);
                    return View(uczens.ToList());
                }
            }
            if (User.IsInRole("Uczeń"))
            {
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                var uczens = db.Uczens.Include(u => u.klasaUcznia).Include(u => u.OcenyUcznia).Where(x => x.Email.Equals(ZalogowanyUczen));
                return View(uczens.ToList());
            }
            return View("~/Views/Uczens/PermissionDenied.cshtml");
        }

        // GET: Uczens/Details/5
        public ActionResult Details(int? id)
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
            return View(uczen);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Uczens/Create
        public ActionResult Create()
        {
            ViewBag.KlasaID = new SelectList(db.Klasas, "ID", "NazwaKlasy");
            return View();
        }

        // POST: Uczens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Imie,Nazwisko,NumerTelefonu,Adres,Email,KlasaID")] Uczen uczen)
        {
            if (ModelState.IsValid)
            {
                db.Uczens.Add(uczen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KlasaID = new SelectList(db.Klasas, "ID", "NazwaKlasy", uczen.KlasaID);
            return View(uczen);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Uczens/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.KlasaID = new SelectList(db.Klasas, "ID", "NazwaKlasy", uczen.KlasaID);
            return View(uczen);
        }

        // POST: Uczens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Imie,Nazwisko,NumerTelefonu,Adres,Email,KlasaID")] Uczen uczen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uczen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KlasaID = new SelectList(db.Klasas, "ID", "NazwaKlasy", uczen.KlasaID);
            return View(uczen);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Uczens/Delete/5
        public ActionResult Delete(int? id)
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
            return View(uczen);
        }

        // POST: Uczens/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uczen uczen = db.Uczens.Find(id);
            db.Uczens.Remove(uczen);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Klasas/CreateOcenasFromUczens
        public ActionResult CreateOcenasFromUczens(int? id)
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
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Email", uczen.ID);
                return View();
            }
            else if (User.IsInRole("Administrator"))
            {
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Email", uczen.ID);
                return View();
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }

        }

        // POST: Klasas/CreateOcenasFromUczens
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOcenasFromUczens([Bind(Include = "ID,WartoscOceny,UczenID,PrzedmiotID")] Ocena ocena)
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
                    ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Email", ocena.UczenID);
                    return View(ocena);
                }
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu", ocena.PrzedmiotID);
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Email", ocena.UczenID);
                return View(ocena);
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

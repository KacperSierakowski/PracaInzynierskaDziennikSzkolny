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
        public ActionResult Index(string searching)
        {
            if (User.IsInRole("Nauczyciel") || User.IsInRole("Administrator"))
            {
                return View(db.Klasas.Include(x => x.UczniowieKlasy).Where(x => x.NazwaKlasy.Contains(searching) || searching == null).ToList());
            }
            else if (User.IsInRole("Uczeń"))
            {
                //var viewModel = new DziennikSzkolny13DB();
                // viewModel.Klasas = db.Klasas.Where(i=>i.UczniowieKlasy.Where(i=>i.Email.Equals("")));
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                //var Zalogowany = db.Uczens.Where(i => i.Email.Equals(ZalogowanyUczen));
                // var KlasaZalogowanego = Zalogowany.Where(w => w.klasaUcznia.NazwaKlasy.ToString());
                return View(db.Klasas.Include(u => u.UczniowieKlasy).Where(x => x.UczniowieKlasy.Any(u => u.Email.Equals(ZalogowanyUczen))).ToList());
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }



        // GET: Klasas/Details/5
        public ActionResult Details(int? id)
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

        public JsonResult IndexForMobileApp()
        {
            var ListaKlas = db.Klasas.Include(x => x.UczniowieKlasy).ToList();
            return Json(ListaKlas, JsonRequestBehavior.AllowGet);
        }
        
        //public IEnumerable<Klasa> Get()
        //{
        //    return db.Klasas.Include(x => x.UczniowieKlasy).ToList();
        //}


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

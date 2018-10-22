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
    public class OcenasController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Ocenas
        public ActionResult Index(string SearchBy, string searching)
        {
            if (User.IsInRole("Administrator"))
            {
                if (SearchBy == "NazwaKlasy")
                {
                    var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen);
                    return View(ocenas.Where(x => x.OcenaUczen.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "NazwiskoUcznia")
                {
                    var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen);
                    return View(ocenas.Where(x => x.OcenaUczen.Nazwisko.Contains(searching) || searching == null).ToList());
                }
                else
                {
                    var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen);
                    return View(ocenas.Where(x => x.OcenaPrzedmiot.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
                }
            }
            else if (User.IsInRole("Nauczyciel"))
            {
                string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];
                if (SearchBy == "NazwaKlasy")
                {
                    var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen)
                        .Where(x => x.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel));
                    return View(ocenas.Where(x => x.OcenaUczen.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "NazwiskoUcznia")
                {
                    var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen)
                        .Where(x => x.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel));
                    return View(ocenas.Where(x => x.OcenaUczen.Nazwisko.Contains(searching) || searching == null).ToList());
                }
                else
                {
                    var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen)
                        .Where(x => x.OcenaPrzedmiot.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel));
                    return View(ocenas.Where(x => x.OcenaPrzedmiot.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
                }
            }
            else if (User.IsInRole("Uczeń"))
            {
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                var ocenas = db.Ocenas.Include(o => o.OcenaPrzedmiot).Include(o => o.OcenaUczen)
                    .Where(x => x.OcenaUczen.Email.Equals(ZalogowanyUczen));
                return View(ocenas.Where(x => x.OcenaPrzedmiot.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }
        public IQueryable<Ocena> GetOcenas()
        {
            return db.Ocenas;
        }
        public IEnumerable<Ocena> Get()
        {
            return db.Ocenas.ToList();
        }
        public JsonResult IndexForMobileApp()
        {
            var ListaOcen = db.Ocenas.Include(x => x.OcenaUczen).ToList();
            return Json(ListaOcen, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("findall")]
        public HttpResponseMessage findAll()
        {
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                //var nauczyciels = db.Ocenas.Include(x => x.OcenaUczen).Select(p => new Nauczyciel
                //{
                //    ID = p.ID,
                //    Imie = p.Imie,
                //    Nazwisko = p.Nazwisko,
                //    Adres = p.Adres,
                //    Email = p.Email,
                //    NumerTelefonu = p.NumerTelefonu,
                //    PrzedmiotyNauczyciela = p.PrzedmiotyNauczyciela,
                //}).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(db.Ocenas));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
        }


        // GET: Ocenas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = db.Ocenas.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Nauczyciel") || User.IsInRole("Administrator"))
            {
                return View(ocena);
            }
            else
            {
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                if (ocena.OcenaUczen.Email == ZalogowanyUczen)
                {
                    return View(ocena);
                }
                else
                {
                    return View("~/Views/Uczens/PermissionDenied.cshtml");
                }
            }
        }

        // GET: Ocenas/Create
        public ActionResult Create()
        {
            if ((User.IsInRole("Nauczyciel")))
            {
                string ZalogowanyNauczyciel = Request.ServerVariables["LOGON_USER"];
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(u => u.przedmiotNauczyciel.Email.Equals(ZalogowanyNauczyciel)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Email");
                return View();
            }
            else if (User.IsInRole("Administrator"))
            {
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Email");
                return View();
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }

        }

        // POST: Ocenas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WartoscOceny,UczenID,PrzedmiotID")] Ocena ocena)
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
                    //return View("~/Views/Klasas/Details/"+ocena.OcenaUczen.KlasaID, ocena);
                    return View("~/Views/Uczens/PermissionDenied.cshtml");
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

        // GET: Ocenas/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Ocena ocena = db.Ocenas.Find(id);
                if (ocena == null)
                {
                    return HttpNotFound();
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

        // POST: Ocenas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WartoscOceny,UczenID,PrzedmiotID")] Ocena ocena)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ocena).State = EntityState.Modified;
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

        // GET: Ocenas/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ocena ocena = db.Ocenas.Find(id);
            if (ocena == null)
            {
                return HttpNotFound();
            }
            return View(ocena);
        }

        // POST: Ocenas/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ocena ocena = db.Ocenas.Find(id);
            db.Ocenas.Remove(ocena);
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

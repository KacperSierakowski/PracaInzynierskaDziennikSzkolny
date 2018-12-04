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
    public class NieobecnoscsController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Nieobecnoscs
        public ActionResult Index(string SearchBy, string searching)
        {
            string Zalogowany = Request.ServerVariables["LOGON_USER"];
            if (User.IsInRole("Administrator"))
            {
                if (SearchBy == "NazwaKlasy")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot);
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "NazwiskoUcznia")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot);
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.Nazwisko.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "Przedmiot")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot);
                    return View(nieobecnoscs.Where(x => x.OpuszczonyPrzedmiot.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
                }
                else//Nauczyciel
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot);
                    return View(nieobecnoscs.Where(x => x.OpuszczonyPrzedmiot.przedmiotNauczyciel.Nazwisko.Contains(searching) || searching == null).ToList());
                }
            }
            else if (User.IsInRole("Nauczyciel"))
            {
                if (SearchBy == "NazwaKlasy")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot)
                        .Where(s => s.OpuszczonyPrzedmiot.przedmiotNauczyciel.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "NazwiskoUcznia")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot)
                        .Where(s => s.OpuszczonyPrzedmiot.przedmiotNauczyciel.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.Nazwisko.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "Przedmiot")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot)
                        .Where(s => s.OpuszczonyPrzedmiot.przedmiotNauczyciel.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.OpuszczonyPrzedmiot.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
                }
                else
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.OpuszczonyPrzedmiot)
                        .Where(s => s.OpuszczonyPrzedmiot.przedmiotNauczyciel.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
            }
            else //uczen
            {
                if (SearchBy == "Przedmiot")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(n => n.OpuszczonyPrzedmiot).Include(n => n.UczenDotyczacy).Include(o => o.OpuszczonyPrzedmiot)
                            .Where(s => s.UczenDotyczacy.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.OpuszczonyPrzedmiot.NazwaPrzedmiotu.Contains(searching) || searching == null).ToList());
                }
                else
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(n => n.OpuszczonyPrzedmiot).Include(n => n.UczenDotyczacy).Include(o => o.OpuszczonyPrzedmiot)
                            .Where(s => s.UczenDotyczacy.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.ToList());
                }
            }
        }

        // GET: Nieobecnoscs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nieobecnosc nieobecnosc = db.Nieobecnoscs.Find(id);
            if (nieobecnosc == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Nauczyciel") || User.IsInRole("Administrator"))
            {
                return View(nieobecnosc);
            }
            else
            {
                string ZalogowanyUczen = Request.ServerVariables["LOGON_USER"];
                if (nieobecnosc.UczenDotyczacy.Email == ZalogowanyUczen)
                {
                    return View(nieobecnosc);
                }
                else
                {
                    return View("~/Views/Uczens/PermissionDenied.cshtml");
                }
            }
        }

        // GET: Nieobecnoscs/Create
        public ActionResult Create()
        {
            if ((User.IsInRole("Nauczyciel")))
            {
                string Zalogowany = Request.ServerVariables["LOGON_USER"];
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(s => s.przedmiotNauczyciel.Email.Equals(Zalogowany)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa");
                return View();
            }
            else if (User.IsInRole("Administrator"))
            {
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots, "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "PelnaNazwa");
                return View();
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // POST: Nieobecnoscs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Data,UczenID,PrzedmiotID,CzyUsprawiedliwiona")] Nieobecnosc nieobecnosc)
        {

            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Nieobecnoscs.Add(nieobecnosc);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                string Zalogowany = Request.ServerVariables["LOGON_USER"];

                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(s => s.przedmiotNauczyciel.Email.Equals(Zalogowany)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie", nieobecnosc.UczenDotyczacy.PelnaNazwa);
                return View(nieobecnosc);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // GET: Nieobecnoscs/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                string Zalogowany = Request.ServerVariables["LOGON_USER"];
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Nieobecnosc nieobecnosc = db.Nieobecnoscs.Find(id);
                if (nieobecnosc == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(s => s.przedmiotNauczyciel.Email.Equals(Zalogowany)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie", nieobecnosc.UczenID);
                return View(nieobecnosc);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // POST: Nieobecnoscs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Data,UczenID,PrzedmiotID,CzyUsprawiedliwiona")] Nieobecnosc nieobecnosc)
        {
            if ((User.IsInRole("Nauczyciel")) || User.IsInRole("Administrator"))
            {
                string Zalogowany = Request.ServerVariables["LOGON_USER"];
                if (ModelState.IsValid)
                {
                    db.Entry(nieobecnosc).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.PrzedmiotID = new SelectList(db.Przedmiots.Where(s => s.przedmiotNauczyciel.Email.Equals(Zalogowany)), "ID", "NazwaPrzedmiotu");
                ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie", nieobecnosc.UczenID);
                return View(nieobecnosc);
            }
            else
            {
                return View("~/Views/Uczens/PermissionDenied.cshtml");
            }
        }

        // GET: Nieobecnoscs/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nieobecnosc nieobecnosc = db.Nieobecnoscs.Find(id);
            if (nieobecnosc == null)
            {
                return HttpNotFound();
            }
            return View(nieobecnosc);
        }

        // POST: Nieobecnoscs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Nieobecnosc nieobecnosc = db.Nieobecnoscs.Find(id);
            db.Nieobecnoscs.Remove(nieobecnosc);
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

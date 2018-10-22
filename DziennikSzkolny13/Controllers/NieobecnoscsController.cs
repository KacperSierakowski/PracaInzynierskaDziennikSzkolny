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
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy);
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "NazwiskoUcznia")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy);
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.Nazwisko.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "Przedmiot")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy);
                    return View(nieobecnoscs.Where(x => x.NauczycielWystawiajacy.PrzedmiotyNauczyciela.Any(s=>s.NazwaPrzedmiotu.Contains(searching) || searching == null)).ToList());
                }
                else//Nauczyciel
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy);
                    return View(nieobecnoscs.Where(x => x.NauczycielWystawiajacy.Nazwisko.Contains(searching) || searching == null).ToList());
                }
            }
            else if (User.IsInRole("Nauczyciel"))
            {
                if (SearchBy == "NazwaKlasy")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy)
                        .Where(s=>s.NauczycielWystawiajacy.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.klasaUcznia.NazwaKlasy.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "NazwiskoUcznia")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy)
                        .Where(s => s.NauczycielWystawiajacy.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.UczenDotyczacy.Nazwisko.Contains(searching) || searching == null).ToList());
                }
                else if (SearchBy == "Przedmiot")
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy)
                        .Where(s => s.NauczycielWystawiajacy.Email.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.NauczycielWystawiajacy.PrzedmiotyNauczyciela.Any(s => s.NazwaPrzedmiotu.Contains(searching) || searching == null)).ToList());
                }
                else
                {
                    var nieobecnoscs = db.Nieobecnoscs.Include(o => o.UczenDotyczacy.klasaUcznia).Include(o => o.NauczycielWystawiajacy)
                        .Where(s => s.NauczycielWystawiajacy.Nazwisko.Equals(Zalogowany));
                    return View(nieobecnoscs.Where(x => x.NauczycielWystawiajacy.Nazwisko.Contains(searching) || searching == null).ToList());
                }
            }
            else //uczen
            {
                var nieobecnoscs = db.Nieobecnoscs.Include(n => n.NauczycielWystawiajacy).Include(n => n.UczenDotyczacy)
                        .Where(s => s.UczenDotyczacy.Email.Equals(Zalogowany));
                return View(nieobecnoscs.ToList());
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
            return View(nieobecnosc);
        }

        // GET: Nieobecnoscs/Create
        public ActionResult Create()
        {
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie");
            ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie");
            return View();
        }

        // POST: Nieobecnoscs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Data,UczenID,NauczycielID")] Nieobecnosc nieobecnosc)
        {
            if (ModelState.IsValid)
            {
                db.Nieobecnoscs.Add(nieobecnosc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie", nieobecnosc.NauczycielID);
            ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie", nieobecnosc.UczenID);
            return View(nieobecnosc);
        }

        // GET: Nieobecnoscs/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie", nieobecnosc.NauczycielID);
            ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie", nieobecnosc.UczenID);
            return View(nieobecnosc);
        }

        // POST: Nieobecnoscs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Data,UczenID,NauczycielID")] Nieobecnosc nieobecnosc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nieobecnosc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NauczycielID = new SelectList(db.Nauczyciels, "ID", "Imie", nieobecnosc.NauczycielID);
            ViewBag.UczenID = new SelectList(db.Uczens, "ID", "Imie", nieobecnosc.UczenID);
            return View(nieobecnosc);
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
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

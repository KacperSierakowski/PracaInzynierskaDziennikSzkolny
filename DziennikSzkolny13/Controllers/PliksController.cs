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
    public class PliksController : Controller
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: Pliks
        public ActionResult Index()
        {
            return View(db.Pliks.ToList());
        }

        // GET: Pliks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plik plik = db.Pliks.Find(id);
            if (plik == null)
            {
                return HttpNotFound();
            }
            return View(plik);
        }

        // GET: Pliks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pliks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NazwaPliku")] Plik plik, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                if (UploadImage != null)
                {
                    //if (UploadImage.ContentType == "image/jpg" || UploadImage.ContentType == "image/png" || UploadImage.ContentType == "image/bmp"
                    //    || UploadImage.ContentType == "image/gif" || UploadImage.ContentType == "image/jpeg")
                    UploadImage.SaveAs(Server.MapPath("/") + "/Content/WgranePliki/" + UploadImage.FileName);
                    plik.UrlPliku = UploadImage.FileName;
                }
                db.Pliks.Add(plik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(plik);
        }

        // GET: Pliks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plik plik = db.Pliks.Find(id);
            if (plik == null)
            {
                return HttpNotFound();
            }
            return View(plik);
        }

        // POST: Pliks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NazwaPliku,UrlPliku")] Plik plik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(plik);
        }

        // GET: Pliks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plik plik = db.Pliks.Find(id);
            if (plik == null)
            {
                return HttpNotFound();
            }
            return View(plik);
        }

        // POST: Pliks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plik plik = db.Pliks.Find(id);
            db.Pliks.Remove(plik);
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

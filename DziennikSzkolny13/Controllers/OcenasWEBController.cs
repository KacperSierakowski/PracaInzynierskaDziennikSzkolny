using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DziennikSzkolny13.Models;

namespace DziennikSzkolny13.Controllers
{
    public class OcenasWEBController : ApiController
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: api/OcenasWEB
        public IQueryable<Ocena> GetOcenas()
        {
            return db.Ocenas;
        }

        // GET: api/OcenasWEB/5
        [ResponseType(typeof(Ocena))]
        public IHttpActionResult GetOcena(int id)
        {
            Ocena ocena = db.Ocenas.Find(id);
            if (ocena == null)
            {
                return NotFound();
            }

            return Ok(ocena);
        }

        // PUT: api/OcenasWEB/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOcena(int id, Ocena ocena)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ocena.ID)
            {
                return BadRequest();
            }

            db.Entry(ocena).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OcenaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/OcenasWEB
        [ResponseType(typeof(Ocena))]
        public IHttpActionResult PostOcena(Ocena ocena)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //TODO: Check if there is user from ocena and przedmiot
            int WartoscOcenyTMP = ocena.WartoscOceny;
            int IDuczniaTMP = ocena.UczenID;
            int IDprzedmiotuTMP = ocena.PrzedmiotID;

            //Ocena ocenaTMP = db.Ocenas.Find(id);
            //if (ocena == null)
            //{
            //    return HttpNotFound();
            //}
            Uczen uczenTMP = db.Uczens.Find(IDuczniaTMP);
            if (uczenTMP == null)
            {
                return NotFound();
            }
            Przedmiot przedmiotTMP = db.Przedmiots.Find(IDprzedmiotuTMP);
            if (przedmiotTMP == null)
            {
                return NotFound();
            }

            db.Ocenas.Add(ocena);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ocena.ID }, ocena);
        }

        // DELETE: api/OcenasWEB/5
        [ResponseType(typeof(Ocena))]
        public IHttpActionResult DeleteOcena(int id)
        {
            Ocena ocena = db.Ocenas.Find(id);
            if (ocena == null)
            {
                return NotFound();
            }

            db.Ocenas.Remove(ocena);
            db.SaveChanges();

            return Ok(ocena);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OcenaExists(int id)
        {
            return db.Ocenas.Count(e => e.ID == id) > 0;
        }
    }
}
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
    public class KlasasWEBController : ApiController
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: api/KlasasWEB
        public IQueryable<KlasaWEB> GetKlasas()
        {
            var ZwracaneKlasy = db.Klasas.Select(
                p => new KlasaWEB
                {
                    ID = p.ID,
                    NazwaKlasy = p.NazwaKlasy,
                    ProfilKlasy = p.ProfilKlasy
                }
            ).AsQueryable();
            return ZwracaneKlasy;
        }

        // GET: api/KlasasWEB/5
        [ResponseType(typeof(Klasa))]
        public IHttpActionResult GetKlasa(int id)
        {
            Klasa klasa = db.Klasas.Find(id);
            if (klasa == null)
            {
                return NotFound();
            }
            KlasaWEB ZwracanaKlasa = new KlasaWEB();
            ZwracanaKlasa.ID = klasa.ID;
            ZwracanaKlasa.NazwaKlasy = klasa.NazwaKlasy;
            ZwracanaKlasa.ProfilKlasy = klasa.ProfilKlasy;
            return Ok(ZwracanaKlasa);
        }

        // PUT: api/KlasasWEB/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKlasa(int id, Klasa klasa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != klasa.ID)
            {
                return BadRequest();
            }

            db.Entry(klasa).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KlasaExists(id))
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

        // POST: api/KlasasWEB
        [ResponseType(typeof(Klasa))]
        public IHttpActionResult PostKlasa(Klasa klasa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Klasas.Add(klasa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = klasa.ID }, klasa);
        }

        // DELETE: api/KlasasWEB/5
        [ResponseType(typeof(Klasa))]
        public IHttpActionResult DeleteKlasa(int id)
        {
            Klasa klasa = db.Klasas.Find(id);
            if (klasa == null)
            {
                return NotFound();
            }

            db.Klasas.Remove(klasa);
            db.SaveChanges();

            return Ok(klasa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KlasaExists(int id)
        {
            return db.Klasas.Count(e => e.ID == id) > 0;
        }
    }
}
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
    public class UczensWEBController : ApiController
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: api/UczensWEB
        public IQueryable<UczenWEB> GetUczens()
        {
            var ZwracaniUczniowie = db.Uczens.Select(
               p => new UczenWEB
               {
                   ID = p.ID,
                   Imie = p.Imie,
                   Nazwisko = p.Nazwisko,
                   NumerTelefonu = p.NumerTelefonu,
                   KlasaID = p.KlasaID,
                   Adres = p.Adres,
                   Email = p.Email,
               }
           ).AsQueryable();
            return ZwracaniUczniowie;
        }

        // GET: api/UczensWEB/5
        [ResponseType(typeof(Uczen))]
        public IHttpActionResult GetUczen(int id)
        {
            Uczen uczen = db.Uczens.Find(id);
            if (uczen == null)
            {
                return NotFound();
            }
            UczenWEB ZwracanyUczen = new UczenWEB();
            ZwracanyUczen.ID = uczen.ID;
            ZwracanyUczen.Imie = uczen.Imie;
            ZwracanyUczen.Nazwisko = uczen.Nazwisko;
            ZwracanyUczen.KlasaID = uczen.KlasaID;
            ZwracanyUczen.NumerTelefonu = uczen.NumerTelefonu;
            ZwracanyUczen.Email = uczen.Email;
            ZwracanyUczen.Adres = uczen.Adres;

            return Ok(ZwracanyUczen);
        }

        // PUT: api/UczensWEB/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUczen(int id, Uczen uczen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uczen.ID)
            {
                return BadRequest();
            }

            db.Entry(uczen).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UczenExists(id))
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

        // POST: api/UczensWEB
        [ResponseType(typeof(Uczen))]
        public IHttpActionResult PostUczen(Uczen uczen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            db.Uczens.Add(uczen);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = uczen.ID }, uczen);
        }

        // DELETE: api/UczensWEB/5
        [ResponseType(typeof(Uczen))]
        public IHttpActionResult DeleteUczen(int id)
        {
            Uczen uczen = db.Uczens.Find(id);
            if (uczen == null)
            {
                return NotFound();
            }

            db.Uczens.Remove(uczen);
            db.SaveChanges();

            return Ok(uczen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UczenExists(int id)
        {
            return db.Uczens.Count(e => e.ID == id) > 0;
        }
    }
}
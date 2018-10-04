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
    public class PrzedmiotsWEBController : ApiController
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();

        // GET: api/PrzedmiotsWEB
        public IQueryable<Przedmiot> GetPrzedmiots()
        {
            return db.Przedmiots;
        }

        // GET: api/PrzedmiotsWEB/5
        [ResponseType(typeof(Przedmiot))]
        public IHttpActionResult GetPrzedmiot(int id)
        {
            Przedmiot przedmiot = db.Przedmiots.Find(id);
            if (przedmiot == null)
            {
                return NotFound();
            }

            return Ok(przedmiot);
        }

        // PUT: api/PrzedmiotsWEB/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPrzedmiot(int id, Przedmiot przedmiot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != przedmiot.ID)
            {
                return BadRequest();
            }

            db.Entry(przedmiot).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrzedmiotExists(id))
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

        // POST: api/PrzedmiotsWEB
        [ResponseType(typeof(Przedmiot))]
        public IHttpActionResult PostPrzedmiot(Przedmiot przedmiot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Przedmiots.Add(przedmiot);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = przedmiot.ID }, przedmiot);
        }

        // DELETE: api/PrzedmiotsWEB/5
        [ResponseType(typeof(Przedmiot))]
        public IHttpActionResult DeletePrzedmiot(int id)
        {
            Przedmiot przedmiot = db.Przedmiots.Find(id);
            if (przedmiot == null)
            {
                return NotFound();
            }

            db.Przedmiots.Remove(przedmiot);
            db.SaveChanges();

            return Ok(przedmiot);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrzedmiotExists(int id)
        {
            return db.Przedmiots.Count(e => e.ID == id) > 0;
        }
    }
}
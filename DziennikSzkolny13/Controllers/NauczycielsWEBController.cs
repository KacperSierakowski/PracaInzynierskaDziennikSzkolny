using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using DziennikSzkolny13.Models;
using Newtonsoft.Json;

namespace DziennikSzkolny13.Controllers
{

    [RoutePrefix("api/NauczycielsWEB")]
    public class NauczycielsWEBController : ApiController
    {
        private DziennikSzkolny13DB db = new DziennikSzkolny13DB();
        
        [HttpGet]
        [Route("findall")]
        public HttpResponseMessage findAll()
        {
            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                //var klasas = db.Klasas.Include(x => x.UczniowieKlasy).Select(p => new Klasa
                //{
                //    ID = p.ID,
                //    NazwaKlasy = p.NazwaKlasy,
                //    ProfilKlasy = p.ProfilKlasy,
                //    UczniowieKlasy = p.UczniowieKlasy
                //}).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(db.Nauczyciels.Include(x => x.PrzedmiotyNauczyciela).ToList()));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
        }

        // GET: api/NauczycielsWEB
        public IQueryable<Nauczyciel> GetNauczyciels()
        {
            return db.Nauczyciels;
        }

        // GET: api/NauczycielsWEB/5
        [ResponseType(typeof(Nauczyciel))]
        public IHttpActionResult GetNauczyciel(int id)
        {
            Nauczyciel nauczyciel = db.Nauczyciels.Find(id);
            if (nauczyciel == null)
            {
                return NotFound();
            }

            return Ok(nauczyciel);
        }

        // PUT: api/NauczycielsWEB/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNauczyciel(int id, Nauczyciel nauczyciel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nauczyciel.ID)
            {
                return BadRequest();
            }

            db.Entry(nauczyciel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NauczycielExists(id))
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

        // POST: api/NauczycielsWEB
        [ResponseType(typeof(Nauczyciel))]
        public IHttpActionResult PostNauczyciel(Nauczyciel nauczyciel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Nauczyciels.Add(nauczyciel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = nauczyciel.ID }, nauczyciel);
        }

        // DELETE: api/NauczycielsWEB/5
        [ResponseType(typeof(Nauczyciel))]
        public IHttpActionResult DeleteNauczyciel(int id)
        {
            Nauczyciel nauczyciel = db.Nauczyciels.Find(id);
            if (nauczyciel == null)
            {
                return NotFound();
            }

            db.Nauczyciels.Remove(nauczyciel);
            db.SaveChanges();

            return Ok(nauczyciel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NauczycielExists(int id)
        {
            return db.Nauczyciels.Count(e => e.ID == id) > 0;
        }
    }
}
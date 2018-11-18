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
                var nauczyciels = db.Nauczyciels.Include(x => x.PrzedmiotyNauczyciela).Select(p => new Nauczyciel
                {
                    ID = p.ID,
                    Imie = p.Imie,
                    Nazwisko = p.Nazwisko,
                    Adres = p.Adres,
                    Email = p.Email,
                    NumerTelefonu = p.NumerTelefonu,
                    PrzedmiotyNauczyciela = p.PrzedmiotyNauczyciela,
                }).ToList();
                response.Content = new StringContent(JsonConvert.SerializeObject(nauczyciels));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }
        }

        // GET: api/NauczycielsWEB
        public IQueryable<NauczycielWEB> GetNauczyciels()
        {
            var ZwracaniNauczyciele = db.Nauczyciels.Select(
             p => new NauczycielWEB
             {
                 ID = p.ID,
                 Imie = p.Imie,
                 Nazwisko = p.Nazwisko,
                 NumerTelefonu = p.NumerTelefonu,
                 Adres = p.Adres,
                 Email = p.Email,
             }
         ).AsQueryable();
            return ZwracaniNauczyciele;
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

            NauczycielWEB ZwracanyNauczyciel = new NauczycielWEB();
            ZwracanyNauczyciel.ID = nauczyciel.ID;
            ZwracanyNauczyciel.Imie = nauczyciel.Imie;
            ZwracanyNauczyciel.Nazwisko = nauczyciel.Nazwisko;
            ZwracanyNauczyciel.NumerTelefonu = nauczyciel.NumerTelefonu;
            ZwracanyNauczyciel.Email = nauczyciel.Email;
            ZwracanyNauczyciel.Adres = nauczyciel.Adres;

            return Ok(ZwracanyNauczyciel);
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
            
            AccountController accountController = new AccountController();
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.Email = nauczyciel.Email;
            loginViewModel.Password = nauczyciel.Nazwisko; string xxx = "Okey";
            System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> XXX = accountController.Login(loginViewModel, xxx);
            
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
using PassionProject_SuyeonJangB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;

namespace PassionProject_SuyeonJangB.Controllers
{
    public class TravelerDataController : ApiController
    {
        //utlizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all Travelers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Travelers in the database, including their associated destinations.
        /// </returns>
        /// <example>
        /// GET: api/TravelerData/ListTravelers
        /// </example>
        [HttpGet]
        [ResponseType(typeof(TravelerDto))]
        public IHttpActionResult ListTravelers()
        {
            List<Traveler> Travelers = db.Travelers.ToList();
            List<TravelerDto> TravelerDtos = new List<TravelerDto>();

            Travelers.ForEach(t => TravelerDtos.Add(new TravelerDto()
            {
                TravelerId = t.TravelerId,
                TravelerFirstName = t.TravelerFirstName,
                TravelerLastName = t.TravelerLastName,
            }));
            return Ok(TravelerDtos);
        }

        /// <summary>
        /// Adds an Traveler to the system
        /// </summary>
        /// <param name="travelers">JSON FORM DATA of an Traveler</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Traveler ID, Traveler Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/TravelerData/AddTraveler
        /// FORM DATA: Traveler JSON Object
        /// </example>
        [ResponseType(typeof(Traveler))]
        [HttpPost]
        public IHttpActionResult AddTraveler(Traveler travelers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Travelers.Add(travelers);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = travelers.TravelerId }, travelers);
        }

        /// <summary>
        /// Returns a Traveler in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Traveler in the system matching up to the Traveler ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Traveler</param>
        /// <example>
        /// GET: api/TravelerData/FindTraveler/5
        /// </example>
        [ResponseType(typeof(TravelerDto))]
        [HttpGet]
        public IHttpActionResult FindTraveler(int id)
        {
            Traveler Traveler = db.Travelers.Find(id);
            TravelerDto TravelerDto = new TravelerDto()
            {
                TravelerId = Traveler.TravelerId,
                TravelerFirstName = Traveler.TravelerFirstName,
                TravelerLastName = Traveler.TravelerLastName,
                TravelerEmail = Traveler.TravelerEmail
            };
            if (Traveler == null)
            {
                return NotFound();
            }

            return Ok(TravelerDto);
        }

        /// <summary>
        /// Updates a particular Traveler in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Traveler ID primary key</param>
        /// <param name="Traveler">JSON FORM DATA of an Traveler</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/TravelerData/UpdateTraveler/5
        /// FORM DATA: Traveler JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTraveler(int id, Traveler Traveler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Traveler.TravelerId)
            {

                return BadRequest();
            }

            db.Entry(Traveler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelerExists(id))
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

        /// <summary>
        /// Deletes an Traveler from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Traveler</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TravelerData/DeleteTraveler/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Traveler))]
        [HttpPost]
        public IHttpActionResult DeleteTraveler(int id)
        {
            Traveler Traveler = db.Travelers.Find(id);
            if (Traveler == null)
            {
                return NotFound();
            }

            db.Travelers.Remove(Traveler);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TravelerExists(int id)
        {
            return db.Travelers.Count(e => e.TravelerId == id) > 0;
        }
    }
}

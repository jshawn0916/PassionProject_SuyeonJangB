using PassionProject_SuyeonJangB.Migrations;
using PassionProject_SuyeonJangB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PassionProject_SuyeonJangB.Controllers
{
    public class DestinationDataController : ApiController
    {
        //utlizing the database connection
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Destinations in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Destinations in the database, including their associated Destinations.
        /// </returns>
        /// <example>
        /// GET: api/DestinationData/ListDestinations
        /// </example>
        [HttpGet]
        [ResponseType(typeof(DestinationDto))]
        public IHttpActionResult ListDestinations()
        {
            List<Destination> Destinations = db.Destinations.ToList();
            List<DestinationDto> DestinationDtos = new List<DestinationDto>();

            Destinations.ForEach(D => DestinationDtos.Add(new DestinationDto()
            {
                DestinationId = D.DestinationId,
                DestinationName = D.DestinationName,
                DestinationCategory = D.DestinationCategory,
                DestinationLocation = D.DestinationLocation,
            }));
            return Ok(DestinationDtos);
        }

        /// <summary>
        /// Grthers informatioon about Destinations related to a particular Journey
        /// </summary>
        /// <returns>
        /// HEADER: 200(Ok)
        /// CONTENT: Returns all Destinations in a database associated with a particular Journey
        /// </returns>
        /// <example>
        /// GET: api/RestaurantData/ListDestinationsforJourney/4
        /// </example>
        [HttpGet]
        public IHttpActionResult ListDestinationsforJourney(int id)
        {
            //sending a query to the database
            //select * from Restaurants...
            List<Destination> Destinations = db.Destinations.Where(
                D => D.Journeys.Any(
                    J => J.JourneyId == id
                )).ToList();

            List<DestinationDto> DestinationDtos = new List<DestinationDto>();

            Destinations.ForEach(D => DestinationDtos.Add(new DestinationDto()
            {
                DestinationId = D.DestinationId,
                DestinationName = D.DestinationName,
                DestinationCategory = D.DestinationCategory,
                DestinationLocation = D.DestinationLocation
            }));

            //push the results to the list of Restaurants to return
            return Ok(DestinationDtos);
        }

        /// <summary>
        /// Returns Destination in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Destination in the system matching up to the Destination ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Destination</param>
        /// <example>
        /// GET: api/DestinationData/FindDestination/5
        /// </example>
        [ResponseType(typeof(DestinationDto))]
        [HttpGet]
        public IHttpActionResult FindDestination(int id)
        {
            Destination Destination = db.Destinations.Find(id);
            DestinationDto DestinationDto = new DestinationDto()
            {
                DestinationId = Destination.DestinationId,
                DestinationName = Destination.DestinationName,
                DestinationCategory = Destination.DestinationCategory,
                DestinationLocation = Destination.DestinationLocation
            };
            if (Destination == null)
            {
                return NotFound();
            }
            Debug.WriteLine(DestinationDto.DestinationId);
            return Ok(DestinationDto);
        }

        /// <summary>
        /// Adds an Destination to the system
        /// </summary>
        /// <param name="Destination">JSON FORM DATA of an Destination</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Destination ID, Destination Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DestinationData/AddDestination
        /// FORM DATA: Species JSON Object
        /// </example>
        [ResponseType(typeof(Destination))]
        [HttpPost]
        public IHttpActionResult AddDestination(Destination Destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Destinations.Add(Destination);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Destination.DestinationId }, Destination);
        }

        /// <summary>
        /// Updates a particular Destination in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Destination ID primary key</param>
        /// <param name="Destination">JSON FORM DATA of an Destination</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DestinationData/UpdateDestination/5
        /// FORM DATA: Destination JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDestination(int id, Destination Destination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Destination.DestinationId)
            {

                return BadRequest();
            }

            db.Entry(Destination).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
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
        /// Deletes an Destination from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Destination</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DestinationData/DeleteDestination/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Destination))]
        [HttpPost]
        public IHttpActionResult DeleteDestination(int id)
        {
            Destination Destination = db.Destinations.Find(id);
            if (Destination == null)
            {
                return NotFound();
            }

            db.Destinations.Remove(Destination);
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

        private bool DestinationExists(int id)
        {
            return db.Destinations.Count(e => e.DestinationId == id) > 0;
        }
    }
}

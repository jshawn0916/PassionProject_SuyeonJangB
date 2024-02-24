using PassionProject_SuyeonJangB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PassionProject_SuyeonJangB.Migrations;


namespace PassionProject_SuyeonJangB.Controllers
{
    public class JourneyDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gathers information about all journeys related to a particular traveler ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all journey in the database, including their associated traveler matched with a particular traveler ID
        /// </returns>
        /// <param name="id">Journey ID.</param>
        /// <example>
        /// GET: api/JourneyData/ListJourneysForTravelers/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(JourneyDto))]
        public IHttpActionResult ListJourneysForTravelers(int id)
        {
            //SQL Equivalent:
            //Select * from Journeys where Journeys.speciesid = {id}
            List<Journey> Journeys = db.Journeys.Where(t => t.TravelerId == id).ToList();
            List<JourneyDto> JourneysDtos = new List<JourneyDto>();

            Journeys.ForEach(j => JourneysDtos.Add(new JourneyDto()
            {
                JourneyId = j.JourneyId,
                JourneyTitle = j.JourneyTitle,
                TravelerId = j.Travelers.TravelerId,
                TravelerFirstName = j.Travelers.TravelerFirstName
            }));

            Debug.WriteLine(JourneysDtos);
            return Ok(JourneysDtos);
        }

        /// <summary>
        /// Adds an Journeys to the system
        /// </summary>
        /// <param name="Journeys">JSON FORM DATA of an Journeys</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Journeys ID, Journeys Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/JourneyData/AddJourney
        /// FORM DATA: Journeys JSON Object
        /// </example>
        [ResponseType(typeof(Journey))]
        [HttpPost]
        public IHttpActionResult AddJourney(Journey Journey)
        {
            Debug.WriteLine(Journey);
            Debug.WriteLine(Journey);
            Debug.WriteLine(Journey);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Journeys.Add(Journey);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Journey.JourneyId }, Journey);
        }

        /// <summary>
        /// Associates a particular Journey with a particular Destinations
        /// </summary>
        /// <param name="JourneyId">The JourneyId primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: DestinationIds
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/JourneyData/AddJourney/3
        /// </example>
        [HttpPost]
        [Route("api/JourneyData/AssociateJourneyWithDestinations/{journeyId}")]
        public IHttpActionResult AssociateJourneyWithDestinations(int journeyId, [FromBody] int[] destinationIds)
        {
            Journey SelectedJourney = db.Journeys.Include(J => J.Destinations).Where(F => F.JourneyId == journeyId).FirstOrDefault();
            if (SelectedJourney == null)
            {
                return NotFound();
            };

            if (destinationIds != null)
            {
                foreach (var destinationId in destinationIds)
                {
                    Destination SelectedDestination = db.Destinations.Find(destinationId);
                    if (SelectedDestination == null)
                    {
                        return NotFound();
                    }
                    //SQL equivalent:
                    //insert into RestaurantsFolderRestaurants (RestaurantsFolderId, RestaurantsId) values ({RestaurantsFolderId},{RestaurantsId})
                    SelectedJourney.Destinations.Add(SelectedDestination);
                }
            }
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// return a Journey to the system
        /// </summary>
        /// <returns>
        /// HEADER: 200(Ok)
        /// Retrieve a Journey from the system matching the primary key JourneyId
        /// or
        /// HEADER: 404(Not Found)
        /// </returns>
        /// <param name="id">The primary key of the Journey</param>
        /// <example>
        /// Get: api/JourneyData/FindJourney/4
        /// </example>
        [ResponseType(typeof(Journey))]
        [HttpGet]
        public IHttpActionResult FindJourney(int id)
        {
            Journey Journey = db.Journeys.Where(J => J.JourneyId == id).FirstOrDefault();

            if (Journey == null)
            {
                return NotFound();
            }

            JourneyDto JourneyDto = new JourneyDto()
            {
                JourneyId = Journey.JourneyId,
                JourneyTitle = Journey.JourneyTitle,
                JourneyExplain = Journey.JourneyExplain,
                TravelerId = Journey.TravelerId
            };

            return Ok(JourneyDto);
        }

        /// <summary>
        /// Removes an association between a particular Journey with a particular Destinations
        /// </summary>
        /// <param name="JourneyID">The JourneyID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/JourneyData/UnAssociateJourneyWithDestinations/2
        /// </example>
        [HttpPost]
        [Route("api/JourneyData/UnAssociateJourneyWithDestinations/{journeyid}")]
        public IHttpActionResult UnAssociateJourneyWithDestinations(int journeyid)
        {
            Journey SelectedJourney = db.Journeys.Include(J => J.Destinations).Where(J => J.JourneyId == journeyid).FirstOrDefault();
            if (SelectedJourney == null)
            {
                return NotFound();
            };
            var DestinationsToRemove = new List<Destination>(SelectedJourney.Destinations);
            foreach (var restaurant in DestinationsToRemove)
            {
                //SQL equivalent:
                //Delete data from the Journeys where all Destinations JourneyId is journeyid in the Journey
                SelectedJourney.Destinations.Remove(restaurant);
            }

            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a particular Journey in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Journey ID primary key</param>
        /// <param name="Journey">JSON FORM DATA of an Journey</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/JourneyData/UpdateJourney/5
        /// FORM DATA: Journey JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateJourney(int id, Journey Journey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Journey.JourneyId)
            {
                return BadRequest();
            }

            db.Entry(Journey).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JourneyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = Journey.JourneyId }, Journey);
        }

        /// <summary>
        /// Deletes a Journey from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Journey</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/JourneyData/DeleteJourney/5
        /// </example>
        [ResponseType(typeof(Journey))]
        [HttpPost]
        public IHttpActionResult DeleteJourney(int id)
        {
            Debug.WriteLine(id);
            Debug.WriteLine(id);
            Debug.WriteLine(id);
            Journey Journey = db.Journeys.Find(id);
            if (Journey == null)
            {
                return NotFound();
            }
            int travelerId = Journey.TravelerId;

            db.Journeys.Remove(Journey);
            db.SaveChanges();
            return Ok(travelerId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JourneyExists(int id)
        {
            return db.Journeys.Count(J => J.JourneyId == id) > 0;
        }

        
    }
}

using PassionProject_SuyeonJangB.Models;
using PassionProject_SuyeonJangB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProject_SuyeonJangB.Controllers
{
    public class JourneyController : Controller
    {
        // GET: Journey
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JourneyController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }
        
        // GET: Journey/List/{id}
        public ActionResult List(int id)
        {
            //objective: communicate with our Journeys data api to retrieve a list of Journeys
            //curl https://localhost:44398/api/Journeysdata/listjourneysfortravelers/2


            string url = "JourneyData/ListJourneysForTravelers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<JourneyDto> ReatedJourneys = response.Content.ReadAsAsync<IEnumerable<JourneyDto>>().Result;
            //Debug.WriteLine("Number of Journeys received : ");
            //Debug.WriteLine(Journeys.Count());

            ViewBag.TravelerId = id;
            return View(ReatedJourneys);
        }

        // GET: Journey/New/{id}
        public ActionResult New(int id)
        {
            // objective: communicate with our Destination data api to retrieve a list of Destinations
            //curl https://localhost:44398/api/DestinationData/ListDestinations


            string url = "DestinationData/ListDestinations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DestinationDto> destinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;

            ViewBag.TravelerId = id;
            return View(destinations);
        }

        // POST: Journey/Create
        [HttpPost]
        public ActionResult Create(Journey Journey, int[] destinationIds)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Journeys.JourneyTitle);
            //objective: add a new journeys into our system using the API
            //curl -H "Content-Type:application/json" -d @journeys.json https://localhost:44398/api/journeysdata/addJourney
            string url = "JourneyData/AddJourney";


            string jsonpayload = jss.Serialize(Journey);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string journeyDto = response.Content.ReadAsStringAsync().Result;
                JourneyDto createdJourney = jss.Deserialize<JourneyDto>(journeyDto);
                int createdJourneyId = createdJourney.JourneyId;

                url = "JourneyData/AssociateJourneyWithDestinations/" + createdJourneyId;
                jsonpayload = jss.Serialize(destinationIds);
                content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";
                response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List/" + Journey.TravelerId);
                }
                else
                {
                    return RedirectToAction("error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Journey/Details/3
        public ActionResult Details(int id)
        {
            DetailsJourney ViewModel = new DetailsJourney();

            //objective: communicate with our user data api to retrieve one Journey
            //curl https://localhost:44398/api/JourneyData/FindJourney/{id}

            string url = "JourneyData/FindJourney/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JourneyDto SelectedJourney = response.Content.ReadAsAsync<JourneyDto>().Result;

            ViewModel.SelectedJourney = SelectedJourney;

            //Show all Destinations under the care of this Journey
            url = "DestinationData/ListDestinationsforJourney/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DestinationDto> JourneyDestinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;

            ViewModel.JourneyDestinations = JourneyDestinations;
            return View(ViewModel);
        }

        // GET: Journey/Edit/5
        public ActionResult Edit(int id)
        {
            EditJourney ViewModel = new EditJourney();

            //objective: communicate with our user data api to retrieve one Folder
            //curl https://localhost:44398/api/JourneyData/FindJourney/{id}

            string url = "JourneyData/FindJourney/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JourneyDto SelectedJourney = response.Content.ReadAsAsync<JourneyDto>().Result;
            ViewModel.SelectedJourney = SelectedJourney;

            //Show all Destinations under the care of this Journey
            url = "DestinationData/ListDestinationsforJourney/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DestinationDto> JourneyDestinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;

            ViewModel.JourneyDestinations = JourneyDestinations;

            //Destinations
            url = "DestinationData/ListDestinations/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<DestinationDto> RegisteredDestinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;

            ViewModel.RegisteredDestinations = RegisteredDestinations;

            return View(ViewModel);
        }

        // POST: Journey/Update/5
        [HttpPost]
        public ActionResult Update(int id, Journey Journey, int[] DestinationIds)
        {

            string url = "JourneyData/UpdateJourney/" + id;
            string jsonpayload = jss.Serialize(Journey);
            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                string JourneyData = response.Content.ReadAsStringAsync().Result;
                JourneyDto updateJourney = jss.Deserialize<JourneyDto>(JourneyData);
                int updateJourneyId = updateJourney.JourneyId;

                url = "JourneyData/UnAssociateJourneyWithDestinations/" + updateJourneyId;
                response = client.PostAsync(url, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    url = "JourneyData/AssociateJourneyWithDestinations/" + updateJourneyId;
                    jsonpayload = jss.Serialize(DestinationIds);
                    content = new StringContent(jsonpayload);
                    content.Headers.ContentType.MediaType = "application/json";
                    response = client.PostAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("List/" + updateJourney.TravelerId);
                    }
                    else
                    {
                        return RedirectToAction("error");
                    }
                }
                else
                {
                    return RedirectToAction("error");
                }
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Journey/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            //objective: communicate with our user data api to retrieve one Journey
            //curl https://localhost:44398/api/JourneyData/FindJourney/{id}
            string url = "JourneyData/FindJourney/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            JourneyDto RestaurantDto = response.Content.ReadAsAsync<JourneyDto>().Result;

            return View(RestaurantDto);
        }

        // POST: Journey/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Debug.WriteLine(id);
            Debug.WriteLine(id);
            Debug.WriteLine(id);
            Debug.WriteLine(id);
            Debug.WriteLine(id);
            string url = "JourneyData/DeleteJourney/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int travelerId = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List/"+ travelerId);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
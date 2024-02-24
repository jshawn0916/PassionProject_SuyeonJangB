using PassionProject_SuyeonJangB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProject_SuyeonJangB.Controllers
{
    public class DestinationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DestinationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }

        // GET: Destination/List
        public ActionResult List()
        {
            // objective: communicate with our Destination data api to retrieve a list of Destinations
            //curl https://localhost:44398/api/DestinationData/ListDestinations


            string url = "DestinationData/ListDestinations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DestinationDto> destinations = response.Content.ReadAsAsync<IEnumerable<DestinationDto>>().Result;
            //Debug.WriteLine("Number of travelers received : ");
            //Debug.WriteLine(travelers.Count());


            return View(destinations);
        }

        // GET: Destination/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Species data api to retrieve one Species
            //curl https://localhost:44398/api/DestinationData/FindDestination/{id}
            string url = "DestinationData/FindDestination/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DestinationDto DestinationDto = response.Content.ReadAsAsync<DestinationDto>().Result;
            Debug.WriteLine(DestinationDto);
            return View(DestinationDto);
        }

        // GET: Destination/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Destination/Create
        [HttpPost]
        public ActionResult Create(Destination Destination)
        {
            //objective: add a new Species into our system using the API
            //curl -H "Content-Type:application/json" -d @Species.json https://localhost:44398/api/DestinationData/AddDestination
            string url = "DestinationData/AddDestination";


            string jsonpayload = jss.Serialize(Destination);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Destination/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "DestinationData/FindDestination/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DestinationDto selectedDestination = response.Content.ReadAsAsync<DestinationDto>().Result;
            return View(selectedDestination);
        }

        // POST: Destination/Update/5
        [HttpPost]
        public ActionResult Update(int id, Destination Destination)
        {

            string url = "DestinationData/UpdateDestination/" + id;

            string jsonpayload = jss.Serialize(Destination);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Destination/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DestinationData/FindDestination/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DestinationDto selectedDestination = response.Content.ReadAsAsync<DestinationDto>().Result;
            return View(selectedDestination);
        }

        // POST: Destination/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DestinationData/DeleteDestination/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
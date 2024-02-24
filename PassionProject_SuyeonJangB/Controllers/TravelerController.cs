using PassionProject_SuyeonJangB.Models;
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
    public class TravelerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static TravelerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }
        // GET: Traveler/List
        public ActionResult List()
        {
            //objective: communicate with our Traveler data api to retrieve a list of Travelers
            //curl https://localhost:44398/api/travelerdata/listtravelers


            string url = "travelerdata/listtravelers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<TravelerDto> travelers = response.Content.ReadAsAsync<IEnumerable<TravelerDto>>().Result;
            //Debug.WriteLine("Number of travelers received : ");
            //Debug.WriteLine(travelers.Count());


            return View(travelers);
        }

        // GET: Traveler/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Traveler/Create
        [HttpPost]
        public ActionResult Create(Traveler Traveler)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Traveler.TravelerName);
            //objective: add a new Traveler into our system using the API
            //curl -H "Content-Type:application/json" -d @Traveler.json https://localhost:44398/api/travelerdata/addtraveler
            string url = "travelerdata/addtraveler";


            string jsonpayload = jss.Serialize(Traveler);
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

        // GET: Traveler/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "TravelerData/FindTraveler/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TravelerDto selectedTraveler = response.Content.ReadAsAsync<TravelerDto>().Result;
            return View(selectedTraveler);
        }

        // POST: Traveler/Update/5
        [HttpPost]
        public ActionResult Update(int id, Traveler Traveler)
        {

            string url = "TravelerData/UpdateTraveler/" + id;

            string jsonpayload = jss.Serialize(Traveler);

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

        // GET: Traveler/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TravelerData/FindTraveler/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TravelerDto selectedTraveler = response.Content.ReadAsAsync<TravelerDto>().Result;
            return View(selectedTraveler);
        }

        // POST: Traveler/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "TravelerData/DeleteTraveler/" + id;
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
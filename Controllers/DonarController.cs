using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP_5212_RNA_Group4_HospitalProject.Models;
using HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Diagnostics;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class DonarController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonarController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        /// <summary>
        /// Returns all Donars in the system
        /// </summary>
        /// <returns>
        /// Content: all donars in the database
        /// </returns>

        // GET: Donar/List
        public ActionResult List()
        {
            string url = "donardata/listdonars";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DonarDto> Donars = response.Content.ReadAsAsync<IEnumerable<DonarDto>>().Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);
            return View(Donars);
        }

        // GET: api/donardata/details/5
        public ActionResult Details(int id)
        {
            DetailsDonar ViewModel = new DetailsDonar();

            string url = "DonarData/FindDonar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DonarDto SelectedDonar = response.Content.ReadAsAsync<DonarDto>().Result;
            ViewModel.SelectedDonar = SelectedDonar;

            // add from notes
          

            return View(ViewModel);
        }

        // GET: Donar/New
        public ActionResult New()
        {
            
            return View();
        }

        // POST: Donar/Create
        [HttpPost]
        public ActionResult Create(Donar Donar)
        {
            string url = "DonarData/adddonar";

            string jsonpayload = jss.Serialize(Donar);
           
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }

        }

        // GET: Donar/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDonar ViewModel = new UpdateDonar();

            string url = "donardata/finddonar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonarDto SelectedDonar = response.Content.ReadAsAsync<DonarDto>().Result;
            ViewModel.SelectedDonar = SelectedDonar;

            return View(ViewModel);
        }

        // POST: Donar/Update/5
        [HttpPost]
        public ActionResult Update(int id, Donar donar)
        {
            string url = "DonarData/UpdateDonar/" + id;
            string jsonpayload = jss.Serialize(donar);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donar/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "donardata/finddonar/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonarDto selecteddonar = response.Content.ReadAsAsync<DonarDto>().Result;
            return View(selecteddonar);

        }

        // POST: Donar/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "donardata/deletedonar/" + id;
            HttpContent content = new StringContent(""); 
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
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

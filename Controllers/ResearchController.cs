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
    public class ResearchController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ResearchController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }
        // GET: Research/List
        public ActionResult List()
        {
            string url = "researchdata/listresearches";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ResearchDto> Researches = response.Content.ReadAsAsync<IEnumerable<ResearchDto>>().Result;
            return View(Researches);
        }

        // GET: Research/Details/5
        public ActionResult Details(int id)
        {
            DetailsResearch ViewModel = new DetailsResearch();

            string url = "ResearchData/FindResearch/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("the response code is ");
            Debug.WriteLine(response.StatusCode);

            ResearchDto SelectedResearch = response.Content.ReadAsAsync<ResearchDto>().Result;
            ViewModel.SelectedResearch = SelectedResearch;
            Debug.WriteLine("Research received: ");
            Debug.WriteLine(SelectedResearch.ResearchName);


            return View(ViewModel);
        }

        // GET: Research/New
        public ActionResult New()
        {
           
            return View();
        }

        // POST: Research/Create
        [HttpPost]
        public ActionResult Create(Research research)
        {
            string url = "ResearchData/addresearch";

            string jsonpayload = jss.Serialize(research);

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

        // GET: Research/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateResearch ViewModel = new UpdateResearch();

            string url = "researchdata/findresearch/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ResearchDto SelectedResearch = response.Content.ReadAsAsync<ResearchDto>().Result;
            ViewModel.SelectedResearch = SelectedResearch;

            return View(ViewModel);
        }

        // POST: Research/Update/5
        [HttpPost]
        public ActionResult Update(int id, Research research)
        {
            string url = "ResearchData/UpdateResearch/" + id;
            string jsonpayload = jss.Serialize(research);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Research/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "researchdata/findresearch/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ResearchDto selectedresearch = response.Content.ReadAsAsync<ResearchDto>().Result;
            return View(selectedresearch);
        }

        // POST: Research/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "researchdata/deleteresearch/" + id;
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

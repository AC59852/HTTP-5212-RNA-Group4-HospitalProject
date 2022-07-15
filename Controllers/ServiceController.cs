using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP_5212_RNA_Group4_HospitalProject.Models;
using HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class ServiceController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ServiceController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // GET: Service
        public ActionResult List()
        {
            string url = "Servicedata/listservices";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ServiceDto> services = response.Content.ReadAsAsync<IEnumerable<ServiceDto>>().Result;

            return View(services);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            DetailsService ViewModel = new DetailsService();

            string url = "Servicedata/findService/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ServiceDto SelectedService = response.Content.ReadAsAsync<ServiceDto>().Result;

            ViewModel.SelectedService = SelectedService;

            return View(ViewModel);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Service/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

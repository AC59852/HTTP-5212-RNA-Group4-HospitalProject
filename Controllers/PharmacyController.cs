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
    public class PharmacyController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PharmacyController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // GET: Pharmacy
        public ActionResult List()
        {
            string url = "pharmacydata/listpharmacies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PharmacyDto> Pharmacies = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;

            return View(Pharmacies);
        }

        // GET: Pharmacy/Details/5
        public ActionResult Details(int id)
        {
            DetailsPharmacy ViewModel = new DetailsPharmacy();

            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PharmacyDto SelectedPharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;

            ViewModel.SelectedPharmacy = SelectedPharmacy;

            url = "prescriptiondata/listprescriptionsforpharmacy" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PrescriptionDto> RelatedPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            ViewModel.RelatedPrescriptions = RelatedPrescriptions;

            return View(ViewModel);
        }

        // GET: Pharmacy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pharmacy/Create
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

        // GET: Pharmacy/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pharmacy/Edit/5
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

        // GET: Pharmacy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pharmacy/Delete/5
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

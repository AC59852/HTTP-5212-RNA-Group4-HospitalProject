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

        //GET: Pharmacy/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Pharmacy/Create
        [HttpPost]
        public ActionResult Create(Pharmacy Pharmacy)
        {
            string url = "pharmacydata/addpharmacy";

            string jsonpayload = jss.Serialize(Pharmacy);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }

        // GET: Pharmacy/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PharmacyDto selectedpharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;

            return View(selectedpharmacy);
        }

        // POST: Pharmacy/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Pharmacy Pharmacy)
        {
            string url = "pharmacydata/updatepharmacy/" + id;
            string jsonpayload = jss.Serialize(Pharmacy);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("/Details/" + Pharmacy.PharmacyID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult DeleteConfirm(int id)
        {
            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PharmacyDto selectedpharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;

            return View(selectedpharmacy);
        }

        // POST: Anime/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "pharmacydata/deletepharmacy/" + id;
            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsJsonAsync(url, content).Result;

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

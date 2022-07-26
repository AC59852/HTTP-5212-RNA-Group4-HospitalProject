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
        /// <summary>
        /// Returns all pharmacies in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all pharmacies in the database
        /// </returns>
        public ActionResult List()
        {
            string url = "pharmacydata/listpharmacies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PharmacyDto> Pharmacies = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;

            return View(Pharmacies);
        }

        /// <summary>
        /// Returns a single pharmacy from the database, 
        /// including the associated prescriptions and staff
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>
        /// CONTENT: A single pharmacy including prescriptions and staff
        /// </returns>
        /// <example>
        /// GET: api/pharmacydata/details/5
        /// </example>
        public ActionResult Details(int id)
        {
            DetailsPharmacy ViewModel = new DetailsPharmacy();

            //objective: communicate with our pharmacy data api to retrieve one pharmacy

            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            // Get the content from the selected pharmacy
            PharmacyDto SelectedPharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;

            // Set the data in the ViewModel to be able to use in render
            ViewModel.SelectedPharmacy = SelectedPharmacy;

            url = "staffdata/liststaffforpharmacy/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> RelatedStaff = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            ViewModel.RelatedStaff = RelatedStaff;


            //show associated prescriptions with this pharmacy
            url = "prescriptiondata/listprescriptionsforpharmacy" + id;

            // Get the prescriptions results related to the chosen pharmacy
            response = client.GetAsync(url).Result;
            IEnumerable<PrescriptionDto> RelatedPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            // Set the data in the ViewModel to be able to use in render
            ViewModel.RelatedPrescriptions = RelatedPrescriptions;

            return View(ViewModel);
        }

        public ActionResult Prescriptions(int id)
        {

            DetailsPharmacy ViewModel = new DetailsPharmacy();

            //objective: communicate with our pharmacy data api to retrieve one pharmacy

            //show associated prescriptions with this pharmacy
            string url = "prescriptiondata/listprescriptionsforpharmacy" + id;

            // Get the prescriptions results related to the chosen pharmacy
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PrescriptionDto> RelatedPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            // Set the data in the ViewModel to be able to use in render
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
        public ActionResult Update(int id, Pharmacy Pharmacy)
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
                Debug.WriteLine(response.StatusCode);
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

        // POST: Pharmacy/Delete/5
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

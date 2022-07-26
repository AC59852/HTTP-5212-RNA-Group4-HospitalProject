using HTTP_5212_RNA_Group4_HospitalProject.Models;
using HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class PrescriptionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PrescriptionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }


        // GET: Prescription
        public ActionResult List()
        {
            //Obj: Communicate with the PrescriptionData API to retrieve the list of prescriptions

            string url = "prescriptiondata/listprescriptions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PrescriptionDto> Prescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            return View(Prescriptions);
        }

        public ActionResult Details(int id)
        {
            DetailsPrescription ViewModel = new DetailsPrescription();

            string url = "prescriptiondata/findprescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PrescriptionDto SelectedCharacter = response.Content.ReadAsAsync<PrescriptionDto>().Result;

            ViewModel.SelectedPrescription = SelectedCharacter;

            return View(ViewModel);
        }

        public ActionResult New()
        {
            string url = "pharmacydata/listpharmacies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PharmacyDto> PharmacyOptions = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;

            return View(PharmacyOptions);
        }

        // POST: Character/Create
        [HttpPost]
        public ActionResult Create(Prescription prescription)
        {
            string url = "prescriptiondata/addprescription";


            string jsonpayload = jss.Serialize(prescription);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");

        }


        public ActionResult Edit(int id)
        {
            UpdatePrescription ViewModel = new UpdatePrescription();

            string url = "prescriptiondata/findprescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PrescriptionDto SelectedPrescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;
            ViewModel.SelectedPrescription = SelectedPrescription;

            url = "pharmacydata/listpharmacies";
            response = client.GetAsync(url).Result;
            IEnumerable<PharmacyDto> PharmacyOptions = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;

            ViewModel.PharmacyOptions = PharmacyOptions;

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Update(int id, Prescription prescription)
        {
            string url = "prescriptiondata/updateprescription/" + id;
            string jsonpayload = jss.Serialize(prescription);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(jsonpayload);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("/Details/" + prescription.PrescriptionID);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult DeleteConfirm(int id)
        {
            string url = "prescriptiondata/findprescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PrescriptionDto selectedprescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;

            return View(selectedprescription);
        }

        // POST: Character/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "prescriptiondata/deleteprescription/" + id;
            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Prescription");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}
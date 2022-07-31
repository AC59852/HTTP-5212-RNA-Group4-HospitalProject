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
        /// <summary>
        /// List returns a webpage containing all of the prescriptions on one page
        /// </summary>
        /// <returns>A page containing basic information about all prescriptions</returns>
        public ActionResult List()
        {
            //Obj: Communicate with the PrescriptionData API to retrieve the list of prescriptions

            // call the function in the PrescriptionDataController.cs file to list all prescriptions
            string url = "prescriptiondata/listprescriptions";

            // Get the data from the call
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Format the data as an object for the view to use
            IEnumerable<PrescriptionDto> Prescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            // Render the view "List" with the data provided
            return View(Prescriptions);
        }

        /// <summary>
        /// Details returns a webpage containing specific information
        /// about a chosen prescription
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>A webpage containing specific information about a chosen prescription</returns>
        public ActionResult Details(int id)
        {
            // Use the Model "DetailsPrescription" as the base model for data that will be used in the view
            DetailsPrescription ViewModel = new DetailsPrescription();

            // call the function in the PrescriptionDataController.cs file to list
            // data about the chosen prescription based on its id
            string url = "prescriptiondata/findprescription/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            // Format the prescription data to be used in the view
            PrescriptionDto SelectedPrescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;
            ViewModel.SelectedPrescription = SelectedPrescription;

            // Render the view with the provided content
            return View(ViewModel);
        }

        /// <summary>
        /// Returns a webpage for the user to create a new prescription
        /// </summary>
        /// <returns>
        /// a webpage for the user to create a new prescription, allowing them to enter data
        /// as well as choose a pharmacy for the prescription, and the staff member who created it
        /// </returns>
        public ActionResult New()
        {
            // Use the Model "UpdatePrescription" as the base model for data that will be used in the view
            UpdatePrescription ViewModel = new UpdatePrescription();

            // call the function in the PharmacyDataController.cs file to list
            // all pharmacies in the database
            string url = "pharmacydata/listpharmacies";

            HttpResponseMessage response = client.GetAsync(url).Result;

            // Format the pharmacy data to be used in the view
            IEnumerable<PharmacyDto> PharmacyOptions = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;
            ViewModel.PharmacyOptions = PharmacyOptions;

            // call the function in the PharmacyDataController.cs file to list
            // all staff in the database
            url = "staffdata/liststaffs";
            response = client.GetAsync(url).Result;

            // Format the staff data to be used in the view
            IEnumerable<StaffDto> StaffOptions = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            ViewModel.StaffOptions = StaffOptions;

            // Render the view with the provided content
            return View(ViewModel);
        }

        /// <summary>
        /// Runs the AddPrescription function in the PrescriptionDataController.cs file
        /// </summary>
        /// <param name="prescription">Prescription Object Model</param>
        /// <returns>A new prescription in the database based on the information provided</returns>
        [HttpPost]
        public ActionResult Create(Prescription prescription)
        {
            // call the function in the PrescriptionDataController.cs file to
            // add a new prescription in the database
            string url = "prescriptiondata/addprescription";

            // Receive the data object and converts it to a JSON string
            string jsonpayload = jss.Serialize(prescription);
            Debug.WriteLine(jsonpayload);

            // Create a new base class containing the data recently turned into a string
            // and provide the appropriate headers for data transfer
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Redirect the user to the List page to see their new addition
            return RedirectToAction("List");

        }

        /// <summary>
        /// Returns a webpage for the user to edit a chosen prescription's information
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>
        /// a webpage for the user to edit a chosen prescription's information
        /// as well as the chosen pharmacy
        /// </returns>
        public ActionResult Edit(int id)
        {
            // Use the Model "UpdatePrescription" as the base model for data that will be used in the view
            UpdatePrescription ViewModel = new UpdatePrescription();

            // calls the function in the PrescriptionDataController.cs file to
            // find the chosen prescription along witih its data
            string url = "prescriptiondata/findprescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Format the staff data to be used in the view
            PrescriptionDto SelectedPrescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;
            ViewModel.SelectedPrescription = SelectedPrescription;

            // call the function in the PharmacyDataController.cs file to list
            // all staff in the database
            url = "pharmacydata/listpharmacies";
            response = client.GetAsync(url).Result;

            // Format the staff data to be used in the view
            IEnumerable<PharmacyDto> PharmacyOptions = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;
            ViewModel.PharmacyOptions = PharmacyOptions;

            // Render the Edit page with content from the chosen prescription
            // to be used as base values in the form
            return View(ViewModel);
        }

        /// <summary>
        /// Returns the UpdatePrescription function in the PrescriptionDataController.cs file
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <param name="prescription">Prescription Object Model</param>
        /// <returns>The chosen prescription updated with the new information</returns>
        [HttpPost]
        public ActionResult Update(int id, Prescription prescription)
        {
            // calls the function in the PrescriptionDataController.cs file to
            // update the prescription with the newly added content
            string url = "prescriptiondata/updateprescription/" + id;

            // Receive the data object and converts it to a JSON string
            string jsonpayload = jss.Serialize(prescription);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // If the changes are successful, redirect the user to that prescriptions details
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("/Details/" + prescription.PrescriptionID);
            }
            // Otherwise, throw an error
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Returns a webpage for confirming the deletion of a chosen prescription
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>a webpage for confirming the deletion of a chosen prescription</returns>
        public ActionResult DeleteConfirm(int id)
        {
            // calls the function in the PrescriptionDataController.cs file to
            // find the chosen prescription along witih its data
            string url = "prescriptiondata/findprescription/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Format the staff data to be used in the view
            PrescriptionDto selectedprescription = response.Content.ReadAsAsync<PrescriptionDto>().Result;

            // Render the page with the provided data
            return View(selectedprescription);
        }

        /// <summary>
        /// Returns the DeletePrescription function from the PrescriptionDataController.cs file
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>Nothing, as the content is being deleted, it removed data from the database</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // calls the function in the PrescriptionDataController.cs file to
            // delete the chosen prescription
            string url = "prescriptiondata/deleteprescription/" + id;
            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // If the delete is successful, redirect to the List page to see the results
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Prescription");
            }
            // Otherwise throw an error
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}
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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";

            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
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
            PharmacyList ViewModel = new PharmacyList();
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

            string url = "pharmacydata/listpharmacies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PharmacyDto> Pharmacies = response.Content.ReadAsAsync<IEnumerable<PharmacyDto>>().Result;

            ViewModel.Pharmacies = Pharmacies;

            return View(ViewModel);
        }

        /// <summary>
        /// Returns a single pharmacy from the database, 
        /// including the associated prescriptions and staff, as well as the staff not currently working here
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>
        /// CONTENT: A single pharmacy including prescriptions and staff both working and not working here
        /// </returns>
        /// <example>
        /// GET: api/Pharmacy/Details/8
        /// </example>
        public ActionResult Details(int id)
        {
            DetailsPharmacy ViewModel = new DetailsPharmacy();
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

            //objective: communicate with our pharmacy data api to retrieve one pharmacy

            // call the function in the PharmacyDataController.cs file to list
            // data about the chosen pharmacy
            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Get the content from the selected pharmacy
            PharmacyDto SelectedPharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;

            // Set the data in the ViewModel to be able to use in render
            ViewModel.SelectedPharmacy = SelectedPharmacy;

            // call the function in the StaffDataController.cs file to list
            // all related staff for the selected pharmacy
            url = "staffdata/liststaffforpharmacy/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> RelatedStaff = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            // Set the data in the ViewModel to be able to use in render
            ViewModel.RelatedStaff = RelatedStaff;

            // call the function in the StaffDataController.cs file to list
            // all available staff for the selected pharmacy
            url = "staffdata/liststaffnotatpharmacy/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<StaffDto> AvailableStaff = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            // Set the data in the ViewModel to be able to use in render
            ViewModel.AvailableStaff = AvailableStaff;


            // call the function in the PrescriptionDataController.cs file to list
            // all related prescriptions for the selected pharmacy
            url = "prescriptiondata/listprescriptionsforpharmacy" + id;

            // Get the prescriptions results related to the chosen pharmacy
            response = client.GetAsync(url).Result;
            IEnumerable<PrescriptionDto> RelatedPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            // Set the data in the ViewModel to be able to use in render
            ViewModel.RelatedPrescriptions = RelatedPrescriptions;

            return View(ViewModel);
        }

        /// <summary>
        /// Returns the prescriptions page containing all prescriptions
        /// related to the selected pharmacy
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>All prescriptions related to the chosen pharmacy</returns>
        /// <example>
        /// api//Pharmacy/Prescriptions/8
        /// </example>
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

        /// <summary>
        /// Associates a chosen staff member with the specific pharmacy
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <param name="StaffId">Staff Primary Key</param>
        /// <returns>A new addition to the bridging table containing the linked Staff ID and the Pharmacy ID</returns>
        /// <example>
        /// api/Pharmacy/Associate/8?StaffId=3
        /// </example>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Associate(int id, int StaffId)
        {
            GetApplicationCookie();//get token credentials

            // call the function in the PharmacyDataController.cs file to
            // associate a chosen staff member with the specific pharmacy
            string url = "pharmacydata/associatepharmacywithstaff/" + id + "/" + StaffId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        /// <summary>
        /// Unassociates a chosen staff member with the specific pharmacy
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <param name="StaffId">Staff Primary Key</param>
        /// <returns>Removing the bridge containing the linked Staff ID and the Pharmacy ID</returns>
        /// <example>
        /// api/Pharmacy/Unassociate/8?StaffId=3
        /// </example>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult UnAssociate(int id, int StaffId)
        {
            GetApplicationCookie();//get token credentials

            // call the function in the PharmacyDataController.cs file to
            // unassociate a chosen staff member with the specific pharmacy
            string url = "pharmacydata/unassociatepharmacywithstaff/" + id + "/" + StaffId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        /// <summary>
        /// Render a webpage for creating a new pharmacy
        /// </summary>
        /// <returns>a webpage for creating a new pharmacy</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Runs the AddPharmacy function in the PharmacyDataController.cs file
        /// </summary>
        /// <param name="Pharmacy">Pharmacy Object Model</param>
        /// <returns>A new pharmacy in the database based on the information provided</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Pharmacy Pharmacy)
        {
            GetApplicationCookie();//get token credentials

            // call the function in the PharmacyDataController.cs file to
            // add a new pharmacy to the database
            string url = "pharmacydata/addpharmacy";

            string jsonpayload = jss.Serialize(Pharmacy);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }

        /// <summary>
        /// Returns a webpage for the user to edit a chosen pharmacy
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>a webpage for the user to edit a chosen pharmacy</returns>
        /// <example>
        /// api/Pharmacy/Edit/8
        /// </example>
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            UpdatePharmacy ViewModel = new UpdatePharmacy();

            // call the function in the PharmacyDataController.cs file to
            // find the specific pharmacy in the database
            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PharmacyDto SelectedPharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;
            ViewModel.SelectedPharmacy = SelectedPharmacy;

            return View(ViewModel);
        }

        /// <summary>
        /// Runs the UpdatePharmacy function in the PharmacyDataController.cs file
        /// If the content is provided and a new image is added, it updates both the pharmacy content and pharmacy image
        /// If only the content is provided with no image, it updates just the pharmacy content
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <param name="PharmacyPic">Pharmacy Image provided in form</param>
        /// <param name="Pharmacy">Pharmacy Object Model</param>
        /// <returns>Updates pharmacy information in the database based on the information provided</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Pharmacy Pharmacy, HttpPostedFileBase PharmacyPic)
        {
            GetApplicationCookie();//get token credentials

            string url = "pharmacydata/updatepharmacy/" + id;
            string jsonpayload = jss.Serialize(Pharmacy);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode && PharmacyPic != null)
            {
                url = "pharmacydata/UploadPharmacyPic/" + id;

                Debug.WriteLine(url);
                Debug.WriteLine(PharmacyPic.InputStream);


                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(PharmacyPic.InputStream);

                requestcontent.Add(imagecontent, "PharmacyPic", PharmacyPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                Debug.WriteLine(response);

                return RedirectToAction("/Details/" + Pharmacy.PharmacyID);
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("/Details/" + Pharmacy.PharmacyID);
            }
            else
            {
                Debug.WriteLine(response.StatusCode);
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Returns a webpage for confirming the deletion of a chosen pharmacy
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>a webpage for confirming the deletion of a chosen pharmacy</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "pharmacydata/findpharmacy/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PharmacyDto selectedpharmacy = response.Content.ReadAsAsync<PharmacyDto>().Result;

            return View(selectedpharmacy);
        }

        /// <summary>
        /// Returns the DeletePharmacy function from the PharmacyDataController.cs file
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>Nothing, as the content is being deleted, it removed data from the database</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials

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

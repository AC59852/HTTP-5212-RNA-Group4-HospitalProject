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
    public class StaffController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StaffController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // GET: Staff
        /// <summary>
        /// Returns all staffs in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all staffs in the database
        /// </returns>
        public ActionResult List()
        {
            string url = "staffdata/liststaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffDto> Staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;

            return View(Staffs);
        }

        /// <summary>
        /// Returns a single data from the database, 
        /// </summary>
        /// <param name="id">Staff Primary Key</param>
        /// <returns>
        /// <example>
        // GET: Staff/Details/5
        /// </example>
        public ActionResult Details(int id)
        {
            DetailsStaff ViewModel = new DetailsStaff();

            string url = "staffdata/findstaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffDto SelectedStaff = response.Content.ReadAsAsync<StaffDto>().Result;

            ViewModel.SelectedStaff = SelectedStaff;

            //url = "prescriptiondata/listprescriptionsforpharmacy" + id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<PrescriptionDto> RelatedPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            //ViewModel.RelatedPrescriptions = RelatedPrescriptions;

            return View(ViewModel);
        }

        //GET: Staff/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        public ActionResult Create(Staff Staff)
        {
            string url = "staffdata/addstaff";

            string jsonpayload = jss.Serialize(Staff);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "staffdata/findstaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffDto selectedstaff = response.Content.ReadAsAsync<StaffDto>().Result;

            return View(selectedstaff);
        }

        // POST: Staff/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Staff Staff)
        {
            string url = "staffdata/updatestaff/" + id;
            string jsonpayload = jss.Serialize(Staff);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("/Details/" + Staff.StaffId);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult DeleteConfirm(int id)
        {
            string url = "staffdata/findstaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffDto selectedstaff = response.Content.ReadAsAsync<StaffDto>().Result;

            return View(selectedstaff);
        }

        // POST: Staff/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "staffdata/deletestaff/" + id;
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
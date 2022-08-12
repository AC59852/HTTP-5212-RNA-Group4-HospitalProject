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
    public class AppointmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        // GET: Appointment
        /// <summary>
        /// Returns all appointments in the system
        /// </summary>
        /// <returns>
        /// CONTENT: all appointments in the database
        /// </returns>
        public ActionResult List()
        {
            string url = "appointmentdata/listappointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> Appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            return View(Appointments);
        }

        /// <summary>
        /// Returns a single appointment from the database, 
        /// </summary>
        /// <param name="id">Appointment Primary Key</param>
        /// <returns>
        /// </returns>
        /// <example>
        // GET: Appointment/Details/5
        /// </example>
        public ActionResult Details(int id)
        {
            DetailsAppointment ViewModel = new DetailsAppointment();

            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            ViewModel.SelectedAppointment = SelectedAppointment;

            //url = "prescriptiondata/listprescriptionsforpharmacy" + id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<PrescriptionDto> RelatedPrescriptions = response.Content.ReadAsAsync<IEnumerable<PrescriptionDto>>().Result;

            //ViewModel.RelatedPrescriptions = RelatedPrescriptions;

            return View(ViewModel);
        }

        /// <summary>
        /// Returns a webpage for the user to create a new appointment
        /// </summary>
        /// <returns>
        /// a webpage for the user to create a new appointment, allowing them to enter data
        /// as well as choose a staff for the appointment
        /// </returns>
        public ActionResult New()
        {
            // Use the Model "AppointmentsVM" as the base model for data that will be used in the view
            AppointmentsVM ViewModel = new AppointmentsVM();

            // call the function in the StaffDataController.cs file to list
            // all staff in the database
            url = "staffdata/liststaffs";
            response = client.GetAsync(url).Result;

            // Format the staff data to be used in the view
            IEnumerable<StaffDto> Staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            ViewModel.Staffs = Staffs;

            // Render the view with the provided content
            return View(ViewModel);
        }

        /// <summary>
        /// Creates a record in the database, 
        /// </summary>
        /// <param name="Appointment">Appointment</param>
        /// <returns>
        /// <example>
        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment Appointment)
        {
            string url = "appointmentdata/addappointment";

            string jsonpayload = jss.Serialize(Appointment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("List");
        }


        /// <summary>
        /// Gets a appointment entity in the database, forupdate using its id
        /// </summary>
        /// <param name="id">appointment Primary Key</param>
        /// <returns>
        /// <example>
        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            // Use the Model "AppointmentsVM" as the base model for data that will be used in the view
            AppointmentsVM ViewModel = new AppointmentsVM();

            // call the function in the StaffDataController.cs file to list
            // all staff in the database
            url = "staffdata/liststaffs";
            response = client.GetAsync(url).Result;

            // Format the staff data to be used in the view
            IEnumerable<StaffDto> Staffs = response.Content.ReadAsAsync<IEnumerable<StaffDto>>().Result;
            ViewModel.Staffs = Staffs;

            //Get actual appoint for the view
            url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            ViewModel.Appointment = selectedappointment;

            return View(ViewModel);
        }

        /// <summary>
        /// Updates an appointment entity in the database, 
        /// </summary>
        /// <param name="id">Appointmnet Primary Key</param>
        /// <param name="Appointment">Appointmnet Object</param>
        /// <returns>
        /// <example>
        // POST: Appointment/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Appointment Appointment)
        {
            string url = "appointmentdata/updateappointment/" + id;
            string jsonpayload = jss.Serialize(Appointment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("/Details/" + Appointment.AppointmentId);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        /// <summary>
        /// Confirm appointment delete from the database, 
        /// </summary>
        /// <param name="id">Appointmnet Primary Key</param>
        /// <returns>
        /// <example>
        public ActionResult DeleteConfirm(int id)
        {
            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(selectedappointment);
        }

        /// <summary>
        /// Deletes an appointment entity from the database, 
        /// </summary>
        /// <param name="id">Appointmnet Primary Key</param>
        /// <returns>
        /// <example>
        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "appointmentdata/deleteappointment/" + id;
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HTTP_5212_RNA_Group4_HospitalProject.Models;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AppointmentData
        [HttpGet]
        [Route("api/AppointmentData/ListAppointments")]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult GetAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentsDtos = new List<AppointmentDto>();

            Appointments.ForEach(ap => AppointmentsDtos.Add(new AppointmentDto()
            {
                AppointmentId = ap.AppointmentId,
                PatientName = ap.PatientName,
                StaffName = ap.Staff.FirstName + " " + ap.Staff.LastName,
                PatientNotes = ap.PatientNotes,
                AppointmentDate = ap.AppointmentDate,
            }));

            return Ok(AppointmentsDtos);
        }

        // GET: api/AppointmentData/5
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        [Route("api/appointmentdata/findappointment/{id}")]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            AppointmentDto AppointmentDto = new AppointmentDto()
            {
                AppointmentId = Appointment.AppointmentId,
                PatientName = Appointment.PatientName,
                StaffName = Appointment.Staff.FirstName + " " + Appointment.Staff.LastName,
                PatientNotes = Appointment.PatientNotes,
                AppointmentDate = Appointment.AppointmentDate,
            };

            if (Appointment == null)
            {
                return NotFound();
            }

            return Ok(AppointmentDto);
        }

        // PUT: api/AppointmentData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/appointmentdata/updateappointment/{id}")]
        public IHttpActionResult PutAppointment(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AppointmentData
        [ResponseType(typeof(Appointment))]
        [Route("api/appointmentdata/addappointment")]
        public IHttpActionResult PostAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.AppointmentId }, appointment);
        }

        // DELETE: api/AppointmentData/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentId == id) > 0;
        }
    }
}
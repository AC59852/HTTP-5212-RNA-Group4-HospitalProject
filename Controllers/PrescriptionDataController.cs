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
    public class PrescriptionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all prescriptions in the database
        /// </summary>
        /// <returns>
        /// CONTENT: All prescriptions in the database
        /// </returns>
        /// <example>
        /// api/PrescriptionData/ListPrescriptions
        /// </example>
        [HttpGet]
        public IEnumerable<PrescriptionDto> ListPrescriptions()
        {
            List<Prescription> Prescriptions = db.Prescriptions.ToList();
            List<PrescriptionDto> PrescriptionDtos = new List<PrescriptionDto>();

            Prescriptions.ForEach(p => PrescriptionDtos.Add(new PrescriptionDto()
            {
                PrescriptionID = p.PrescriptionID,
                PatientName = p.PatientName,
                PharmacyName = p.Pharmacy.PharmacyName
            }));

            return PrescriptionDtos;
        }

        /// <summary>
        /// List all prescriptions for a chosen pharmacy
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>
        /// CONTENT: All prescriptions related to the chosen pharmacy
        /// </returns>
        /// <example>
        /// api/PrescriptionData/ListPrescriptionsForPharmacy/8
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PrescriptionDto))]
        [Route("api/prescriptiondata/listprescriptionsforpharmacy/{id}")]
        public IHttpActionResult ListPrescriptionsForPharmacy(int id)
        {
            List<Prescription> Prescriptions = db.Prescriptions.Where(p => p.PharmacyID == id).ToList();
            List<PrescriptionDto> PrescriptionDtos = new List<PrescriptionDto>();

            Prescriptions.ForEach(p => PrescriptionDtos.Add(new PrescriptionDto()
            {
                PrescriptionID = p.PrescriptionID,
                PatientName= p.PatientName,
                PrescriptionDosage = p.PrescriptionDosage,
                PrescriptionDrug = p.PrescriptionDrug,
                PrescriptionInstructions = p.PrescriptionInstructions,
                PrescriptionRefills = p.PrescriptionRefills,
                PharmacyID = p.Pharmacy.PharmacyID,
                PharmacyName = p.Pharmacy.PharmacyName
            }));

            return Ok(PrescriptionDtos);
        }

        /// <summary>
        /// Returns a chosen prescription in the database
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>
        /// CONTENT: A specific prescription in the database
        /// </returns>
        /// <example>
        /// api/PrescriptionData/FindPrescription/8
        /// </example>
        [ResponseType(typeof(Prescription))]
        [HttpGet]
        [Route("api/Prescriptiondata/findPrescription/{id}")]
        public IHttpActionResult FindPrescription(int id)
        {
            Prescription Prescription = db.Prescriptions.Find(id);
            PrescriptionDto PrescriptionDto = new PrescriptionDto()
            {
                PrescriptionID = Prescription.PrescriptionID,
                PatientName = Prescription.PatientName,
                PrescriptionDosage = Prescription.PrescriptionDosage,
                PrescriptionDrug = Prescription.PrescriptionDrug,
                PrescriptionRefills= Prescription.PrescriptionRefills,
                PrescriptionInstructions = Prescription.PrescriptionInstructions,
                PharmacyID = Prescription.Pharmacy.PharmacyID,
                PharmacyName = Prescription.Pharmacy.PharmacyName,
                StaffId = Prescription.Staff.StaffId,
                FirstName = Prescription.Staff.FirstName,
                LastName = Prescription.Staff.LastName,
                Title = Prescription.Staff.Title,
            };

            if (Prescription == null)
            {
                return NotFound();
            }

            return Ok(PrescriptionDto);
        }

        /// <summary>
        /// Updates a chosen prescription in the database using POST data provided
        /// </summary>
        /// <param name="prescription">Prescription Object Model with data from form</param>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>
        /// CONTENT: Newly updated information for the specific prescription
        /// </returns>
        /// <example>
        /// api/PrescriptionData/UpdatePrescription/8
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/prescriptiondata/updateprescription/{id}")]
        public IHttpActionResult UpdatePrescription(int id, Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != prescription.PrescriptionID)
            {
                return BadRequest();
            }

            db.Entry(prescription).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionExists(id))
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

        /// <summary>
        /// Adds a prescription into the database
        /// </summary>
        /// <param name="prescription">Prescription Object Model with data from form</param>
        /// <returns>
        /// CONTENT: A new prescription with provided POST data
        /// </returns>
        /// <example>
        /// api/PrescriptionData/AddPrescription
        /// </example>
        [ResponseType(typeof(Prescription))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/prescriptiondata/addprescription")]
        public IHttpActionResult AddPrescription(Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prescriptions.Add(prescription);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = prescription.PrescriptionID }, prescription);
        }

        /// <summary>
        /// Deletes a prescription from the system by it's ID.
        /// </summary>
        /// <param name="id">Prescription Primary Key</param>
        /// <returns>
        /// CONTENT: No content, as the prescription is being deleted
        /// </returns>
        /// <example>
        /// POST: api/PrescriptionData/DeletePrescription/8
        /// </example>
        [ResponseType(typeof(Prescription))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/prescriptiondata/deleteprescription/{id}")]
        public IHttpActionResult DeletePrescription(int id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return NotFound();
            }

            db.Prescriptions.Remove(prescription);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrescriptionExists(int id)
        {
            return db.Prescriptions.Count(e => e.PrescriptionID == id) > 0;
        }
    }
}
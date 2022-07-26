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

        // GET: api/PrescriptionData
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

        // GET: api/PrescriptionData/5
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
                PharmacyName = Prescription.Pharmacy.PharmacyName
            };

            if (Prescription == null)
            {
                return NotFound();
            }

            return Ok(PrescriptionDto);
        }

        [ResponseType(typeof(void))]
        [HttpPost]
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


        [ResponseType(typeof(Prescription))]
        [HttpPost]
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

        [ResponseType(typeof(Prescription))]
        [HttpPost]
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
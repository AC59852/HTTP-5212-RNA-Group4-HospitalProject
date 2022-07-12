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
    public class PharmacyDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PharmacyData
        [HttpGet]
        [Route("api/PharmacyData/ListPharmacies")]
        [ResponseType(typeof(PharmacyDto))]
        public IHttpActionResult GetPharmacies()
        {
            List<Pharmacy> Pharmacies = db.Pharmacies.ToList();
            List<PharmacyDto> PharmaciesDtos = new List<PharmacyDto>();

            Pharmacies.ForEach(ph => PharmaciesDtos.Add(new PharmacyDto()
            {
                PharmacyID = ph.PharmacyID,
                PharmacyName = ph.PharmacyName,
                PharmacyLocation = ph.PharmacyLocation,
                PharmacyWaitTime = ph.PharmacyWaitTime,
                PharmacyOpenTime = ph.PharmacyOpenTime,
                PharmacyCloseTime = ph.PharmacyCloseTime,
                PharmacyDelivery = ph.PharmacyDelivery
            }));

            return Ok(PharmaciesDtos);
        }

        // GET: api/PharmacyData/5
        [ResponseType(typeof(Pharmacy))]
        public IHttpActionResult GetPharmacy(int id)
        {
            Pharmacy pharmacy = db.Pharmacies.Find(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            return Ok(pharmacy);
        }

        // PUT: api/PharmacyData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPharmacy(int id, Pharmacy pharmacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pharmacy.PharmacyID)
            {
                return BadRequest();
            }

            db.Entry(pharmacy).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PharmacyExists(id))
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

        // POST: api/PharmacyData
        [ResponseType(typeof(Pharmacy))]
        public IHttpActionResult PostPharmacy(Pharmacy pharmacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pharmacies.Add(pharmacy);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pharmacy.PharmacyID }, pharmacy);
        }

        // DELETE: api/PharmacyData/5
        [ResponseType(typeof(Pharmacy))]
        public IHttpActionResult DeletePharmacy(int id)
        {
            Pharmacy pharmacy = db.Pharmacies.Find(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            db.Pharmacies.Remove(pharmacy);
            db.SaveChanges();

            return Ok(pharmacy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PharmacyExists(int id)
        {
            return db.Pharmacies.Count(e => e.PharmacyID == id) > 0;
        }
    }
}
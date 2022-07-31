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
using System.Diagnostics;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class DonarDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the donar list in the system
        /// </summary>
        /// <returns>
        /// Header: 200 (OK)
        /// Content: all donars in the database
        /// </returns>
        /// <example>
        /// GET: api/DonarData/ListDonars
        /// </example>

        // GET: api/DonarData
        [HttpGet]
        public IEnumerable<DonarDto> ListDonars()
        {
            List<Donar> Donars = db.Donars.ToList();
            List<DonarDto> DonarsDtos = new List<DonarDto>();

            Donars.ForEach(d => DonarsDtos.Add(new DonarDto()
            {
                DonarID = d.DonarID,
                DonarName = d.DonarName,
                DonarBio = d.DonarBio,
                Donation = d.Donation,
                ResearchID = d.Research.ResearchID,
                ResearchName =  d.Research.ResearchName
            }));

            return DonarsDtos;
        }

        // create Listdonarsforresearch method

       
        [ResponseType(typeof(DonarDto))]
        [HttpGet]
        [Route("api/DonarData/FindDonar/{id}")]
        public IHttpActionResult FindDonar(int id)
        {
            Donar Donar =db.Donars.Find(id);
            DonarDto DonarDto = new DonarDto()
            {
                DonarID = Donar.DonarID,
                DonarName = Donar.DonarName,
                DonarBio = Donar.DonarBio,
                Donation = Donar.Donation,
                ResearchID = Donar.Research.ResearchID,
                ResearchName = Donar.Research.ResearchName
            };

            if (Donar == null)
            {
                return NotFound();
            }

            return Ok(DonarDto);
        }

        // POST: api/DonarData/UpdateDonar/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/donardata/updatedata/{id}")]
        public IHttpActionResult UpdateDonar(int id, Donar donar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donar.DonarID)
            {
                return BadRequest();
            }

            db.Entry(donar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonarExists(id))
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

        // POST: api/DonarData/AddDonar
        [ResponseType(typeof(Donar))]
        [HttpPost]
        [Route("api/donardata/adddonar")]
        public IHttpActionResult AddDonar(Donar donar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donars.Add(donar);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donar.DonarID }, donar);
        }


        // DELETE: api/DonarData/DeleteDonar/5
        [ResponseType(typeof(Donar))]
        [HttpPost]
        public IHttpActionResult DeleteDonar(int id)
        {
            Donar donar = db.Donars.Find(id);
            if (donar == null)
            {
                return NotFound();
            }

            db.Donars.Remove(donar);
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

        private bool DonarExists(int id)
        {
            return db.Donars.Count(e => e.DonarID == id) > 0;
        }
    }
}
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
    public class ResearchDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the research list in the system
        /// </summary>
        /// <returns>
        /// Header: 200 (OK)
        /// Content: all research data in the database
        /// </returns>
        /// <example>
        /// GET: api/ResearchData/ListResearches
        /// </example>

        // GET: api/ResearchData
        [HttpGet]
        [Route("api/ResearchData/ListResearches")]
        [ResponseType(typeof(ResearchDto))]
        public IEnumerable<ResearchDto> ListResearches()
        {
            List<Research> Researches = db.Researches.ToList();
            List<ResearchDto> ResearchDtos = new List<ResearchDto>();

            Researches.ForEach(r => ResearchDtos.Add(new ResearchDto()
            {   
                ResearchID = r.ResearchID,
                ResearchName = r.ResearchName,
                ResearchHead = r.ResearchHead,
                ResearchDesc = r.ResearchDesc,
                NoOfCohorts = r.NoOfCohorts,
                StaffId = r.Staff.StaffId,
                FirstName = r.Staff.FirstName,
                LastName = r.Staff.LastName,
                Title = r.Staff.Title

            }));

            return ResearchDtos;
        }

        // create Liststaffforresearch method

        // create listresearchfordonar method

        [HttpGet]
        [ResponseType(typeof(ResearchDto))]

        public IHttpActionResult ListResearchForDonar(int id)
        {
            List<Research> Researches = db.Researches.Where(
                r => r.Donars.Any(
                    d => d.DonarID == id)
                ).ToList();
            List<ResearchDto> ResearchDtos = new List<ResearchDto>();

            Researches.ForEach(r => ResearchDtos.Add(new ResearchDto()
            {
                ResearchID = r.ResearchID,
                ResearchName = r.ResearchName,
                ResearchHead = r.ResearchHead,
                ResearchDesc = r.ResearchDesc,
                NoOfCohorts = r.NoOfCohorts,
            }));

            return Ok(ResearchDtos);
        }




        // GET: api/ResearchData/5
        [HttpGet]
        [ResponseType(typeof(Research))]
        [Route("api/ResearchData/findResearch/{id}")]
        public IHttpActionResult FindResearch(int id)
        {
            Research Research = db.Researches.Find(id);
            ResearchDto ResearchDto = new ResearchDto()
            {
                ResearchID = Research.ResearchID,
                ResearchName = Research.ResearchName,
                ResearchHead = Research.ResearchHead,
                ResearchDesc = Research.ResearchDesc,
                NoOfCohorts = Research.NoOfCohorts,
                StaffId = Research.Staff.StaffId,
                FirstName = Research.Staff.FirstName,
                LastName = Research.Staff.LastName,
                Title = Research.Staff.Title
            };

            if (Research == null)
            {
                return NotFound();
            }

            return Ok(ResearchDto);
        }

        // POST: api/ResearchData/UpdateResearch/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/ResearchData/updateresearch/{id}")]
        public IHttpActionResult UpdateResearch(int id, Research research)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != research.ResearchID)
            {
                return BadRequest();
            }

            db.Entry(research).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResearchExists(id))
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

        // POST: api/ResearchData/AddResearch
        [ResponseType(typeof(Research))]
        [HttpPost]
        [Route("api/ResearchData/addresearch")]
        public IHttpActionResult AddResearch(Research research)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Researches.Add(research);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = research.ResearchID }, research);
        }

        // DELETE: api/ResearchData/DeleteResearch/5
        [ResponseType(typeof(Research))]
        [HttpPost]
        [Route("api/researchdata/deleteresearch/{id}")]
        public IHttpActionResult DeleteResearch(int id)
        {
            Research research = db.Researches.Find(id);
            if (research == null)
            {
                return NotFound();
            }

            db.Researches.Remove(research);
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

        private bool ResearchExists(int id)
        {
            return db.Researches.Count(e => e.ResearchID == id) > 0;
        }
    }
}
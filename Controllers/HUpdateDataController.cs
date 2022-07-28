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
    public class HUpdateDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // *********************** List of all updates *********************
        // *********************** List of all updates *********************

        /// <summary>
        /// Returns all updates present in hospital database
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA: List of all HUpdate and information related to them
        /// </returns>
        /// <example>
        /// GET: api/HUpdateData/ListHUpdates        
        /// </example>

        [HttpGet]
        [Route("api/HUpdateData/ListHUpdates")]
        public IQueryable<HUpdate> ListHUpdates()
        {

            return db.HUpdates;
        }


        // ************** Find Update of given Id *************************
        // ************** Find Update of given Id *************************

        /// <summary>
        /// Returns an update of given id present in hospital database
        /// </summary>
        /// <param name="id">id for HUpdate which needs to be find</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA:  all information of HUpdate with given id
        /// </returns>
        /// <example>
        /// GET: api/HUpdateData/FindHUpdate/5      
        /// </example>

        [HttpGet]
        [Route("api/HUpdateData/FindHUpdate/{id}")]
        [ResponseType(typeof(HUpdate))]
        public IHttpActionResult FindHUpdate(int id)
        {
            HUpdate hUpdate = db.HUpdates.Find(id);

            if (hUpdate == null)
            {
                return NotFound();
            }

            return Ok(hUpdate);
        }


        // ************ Edit an Update of given Id *************************
        // ************ Edit an Update of given Id *************************

        /// <summary>
        /// Perform an edit on HUpdate data of given id present in database
        /// </summary>
        /// <param name="id">id of HUpdate on which edit is being performed</param>
        /// <param name="hUpdate">an HUpdate object containing edited infromation</param>
        /// <returns>
        /// HEADER: 200 (OK) or Bad request (500) etc
        /// DATA: No data is returned
        /// </returns>
        /// <example>
        /// POST: api/HUpdateData/EditHUpdate/1        
        /// </example>

        [HttpPost]
        [Route("api/HUpdateData/EditHUpdate/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult EditHUpdate(int id, HUpdate hUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hUpdate.HUpdateId)
            {
                return BadRequest();
            }

            db.Entry(hUpdate).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HUpdateExists(id))
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


        // ****************** Add a new Update *************************
        // ****************** Add a new Update *************************

        /// <summary>
        /// adds a new HUpdate with its information in database
        /// </summary>
        /// <param name="hUpdate">an HUpdate object containing new info of new Update or event</param>
        /// <returns>
        /// HEADER: 200 (OK) or Bad request (500) etc
        /// DATA: no data is returned
        /// </returns>
        /// <example>
        /// POST: api/HUpdateData/AddHUpdate      
        /// </example>

        [HttpPost]
        [Route("api/HUpdateData/AddHUpdate")]
        [ResponseType(typeof(HUpdate))]
        public IHttpActionResult AddHUpdate(HUpdate hUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HUpdates.Add(hUpdate);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hUpdate.HUpdateId }, hUpdate);
        }


        // **************** Delete Update of given Id ********************
        // **************** Delete Update of given Id ********************


        /// <summary>
        /// Deletes all information of HUpdate of given id
        /// </summary>
        /// <param name="id">id of HUpdate which needs to be deleted</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// DATA: no data is returned
        /// </returns>
        /// <example>
        /// POST: api/HUpdateData/DeleteHUpdate/1      
        /// </example>
       
        [HttpPost]
        [Route("api/HUpdateData/DeleteHUpdate/{id}")]
        [ResponseType(typeof(HUpdate))]
        public IHttpActionResult DeleteHUpdate(int id)
        {
            HUpdate hUpdate = db.HUpdates.Find(id);
            if (hUpdate == null)
            {
                return NotFound();
            }

            db.HUpdates.Remove(hUpdate);
            db.SaveChanges();

            return Ok(hUpdate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HUpdateExists(int id)
        {
            return db.HUpdates.Count(e => e.HUpdateId == id) > 0;
        }
    }
}
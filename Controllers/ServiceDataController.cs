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
    public class ServiceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ServiceData/listservice
        [HttpGet]
        [Route("api/ServiceData/ListServices")]
        [ResponseType(typeof(ServiceDto))]
        public IHttpActionResult GetServices()
        {
            List<Service> Services = db.Services.ToList();
            List<ServiceDto> ServicesDtos = new List<ServiceDto>();

            Services.ForEach(s => ServicesDtos.Add(new ServiceDto()
            {
                ServiceID = s.ServiceID,
                ServiceName = s.ServiceName,
                ServiceDescription = s.ServiceDescription,
                ServiceImage = s.ServiceImage,
               
            }));

            return Ok(ServicesDtos);
        }

        // GET: api/ServiceData/findservice/5
        [HttpGet]
        [ResponseType(typeof(ServiceDto))]
        [Route("api/Servicedata/findService/{id}")]
        public IHttpActionResult FindService(int id)
        {
            Service Service = db.Services.Find(id);
            ServiceDto ServiceDto = new ServiceDto()
            {
                ServiceID = Service.ServiceID,
                ServiceName = Service.ServiceName,
                ServiceDescription = Service.ServiceDescription,
                ServiceImage = Service.ServiceImage,
              
            };

            if (Service == null)
            {
                return NotFound();
            }

            return Ok(ServiceDto);
        }

        // PUT: api/ServiceData/updateservice/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/Servicedata/updateService/{id}")]
        public IHttpActionResult PutService(int id, Service Service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Service.ServiceID)
            {
                return BadRequest();
            }

            db.Entry(Service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        // POST: api/ServiceData/addservice
        [ResponseType(typeof(Service))]
        [Route("api/Servicedata/addService")]
        public IHttpActionResult PostService(Service Service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Services.Add(Service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Service.ServiceID }, Service);
        }

        // DELETE: api/ServiceData/deleteservice/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service Service = db.Services.Find(id);
            if (Service == null)
            {
                return NotFound();
            }

            db.Services.Remove(Service);
            db.SaveChanges();

            return Ok(Service);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.ServiceID == id) > 0;
        }
    }
}
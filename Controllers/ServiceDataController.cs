using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
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
        public IHttpActionResult updateService(int id, Service Service)
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

            // Picture update is handled by another method
            db.Entry(Service).Property(s => s.ServiceImage).IsModified = false;
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
        [HttpPost]
        [Route("api/ServiceData/deleteservice/{id}")]
        public IHttpActionResult DeleteService(int id)
        {
            Service Service = db.Services.Find(id);
            if (Service == null)
            {
                return NotFound();
            }
            /*
            if ( Service.ServiceImage != "" || Service.ServiceImage != null )
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Services/" + id + "." + Service.ServiceImage);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }*/

            db.Services.Remove(Service);
            db.SaveChanges();

            return Ok(Service);
        }

        /// <summary>
        /// Receives service picture data, uploads it to the webserver
        /// </summary>
        /// <param name="id">the service id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F serviceimage=@file.jpg "https://localhost:xx/api/servicedata/uploadserviceimage/2"
        /// POST: api/serviceData/Updateserviceimage/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadServiceImage(int id)
        {

           
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                
                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var serviceImage = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (serviceImage.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(serviceImage.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/services/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Services/"), fn);
                                Debug.WriteLine(path);

                                //save the file
                                serviceImage.SaveAs(path);

                                //if these are all successful then we can set these fields
                                picextension = extension;

                                //Update the service haspic and picextension fields in the database
                                Service Selectedservice = db.Services.Find(id);
                                Selectedservice.ServiceImage = extension;
                                db.Entry(Selectedservice).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                              
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

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
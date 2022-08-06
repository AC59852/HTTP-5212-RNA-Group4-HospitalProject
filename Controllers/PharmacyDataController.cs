using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using HTTP_5212_RNA_Group4_HospitalProject.Models;

namespace HTTP_5212_RNA_Group4_HospitalProject.Controllers
{
    public class PharmacyDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all pharmacies in the database
        /// </summary>
        /// <returns>
        /// CONTENT: All pharmacies in the database
        /// </returns>
        /// <example>
        /// api/PharmacyData/ListPharmacies
        /// </example>
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
                PharmacyDelivery = ph.PharmacyDelivery,
                PharmacyHasPic = ph.PharmacyHasPic,
                PicExtension = ph.PicExtension
            }));

            return Ok(PharmaciesDtos);
        }

        /// <summary>
        /// Returns a chosen pharmacy in the database
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>
        /// CONTENT: A specific pharmacy in the database
        /// </returns>
        /// <example>
        /// api/PharmacyData/FindPharmacy/8
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PharmacyDto))]
        [Route("api/pharmacydata/findpharmacy/{id}")]
        public IHttpActionResult FindPharmacy(int id)
        {
            Pharmacy Pharmacy = db.Pharmacies.Find(id);
            PharmacyDto PharmacyDto = new PharmacyDto()
            {
                PharmacyID = Pharmacy.PharmacyID,
                PharmacyName = Pharmacy.PharmacyName,
                PharmacyLocation = Pharmacy.PharmacyLocation,
                PharmacyWaitTime = Pharmacy.PharmacyWaitTime,
                PharmacyOpenTime = Pharmacy.PharmacyOpenTime,
                PharmacyCloseTime = Pharmacy.PharmacyCloseTime,
                PharmacyDelivery = Pharmacy.PharmacyDelivery,
                PharmacyHasPic = Pharmacy.PharmacyHasPic,
                PicExtension = Pharmacy.PicExtension
            };

            if (Pharmacy == null)
            {
                return NotFound();
            }

            return Ok(PharmacyDto);
        }

        /// <summary>
        /// Updates a chosen pharmacy in the database using POST data provided
        /// </summary>
        /// <param name="pharmacy">Pharmacy Object Model with data from form</param>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>
        /// CONTENT: Newly updated information for the specific pharmacy
        /// </returns>
        /// <example>
        /// api/PharmacyData/UpdatePharmacy/8
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/pharmacydata/updatepharmacy/{id}")]
        public IHttpActionResult UpdatePharmacy(int id, Pharmacy pharmacy)
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

            db.Entry(pharmacy).Property(p => p.PharmacyHasPic).IsModified = false;
            db.Entry(pharmacy).Property(p => p.PicExtension).IsModified = false;

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

        /// <summary>
        /// Adds a pharmacy into the database
        /// </summary>
        /// <param name="pharmacy">Pharmacy Object Model with data from form</param>
        /// <returns>
        /// CONTENT: A new pharmacy with provided POST data
        /// </returns>
        /// <example>
        /// api/PharmacyData/AddPharmacy
        /// </example>
        [ResponseType(typeof(Pharmacy))]
        [Route("api/pharmacydata/addpharmacy")]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Deletes a pharmacy from the system by it's ID.
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>
        /// CONTENT: No content, as the pharmacy is being deleted
        /// </returns>
        /// <example>
        /// POST: api/PharmacyData/DeletePharmacy/8
        /// </example>
        [ResponseType(typeof(Pharmacy))]
        [HttpPost]
        [Route("api/pharmacydata/deletepharmacy/{id}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeletePharmacy(int id)
        {
            Pharmacy pharmacy = db.Pharmacies.Find(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            if (pharmacy.PharmacyHasPic && pharmacy.PicExtension != "")
            {
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Pharmacies" + id + "." + pharmacy.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists, deleting data");
                    System.IO.File.Delete(path);
                }
            }

            db.Pharmacies.Remove(pharmacy);
            db.SaveChanges();

            return Ok(pharmacy);
        }

        /// <summary>
        /// Associates a chosen staff member with a specific pharmacy
        /// </summary>
        /// <param name="PharmacyID">Pharmacy Primary Key</param>
        /// <param name="StaffId">Staff Primary Key</param>
        /// <returns>
        /// CONTENT: A new bridge between a staff member and pharmacy
        /// </returns>
        /// <example>
        /// POST api/PharmacyData/AssociatePharmacyWithStaff/8/3
        /// </example>
        [HttpPost]
        [Route("api/PharmacyData/AssociatePharmacyWithStaff/{PharmacyID}/{StaffId}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AssociatePharmacyWithStaff(int PharmacyID, int StaffId)
        {

            Pharmacy SelectedPharmacy = db.Pharmacies.Include(p => p.Staffs).Where(p => p.PharmacyID == PharmacyID).FirstOrDefault();
            Staff SelectedStaff = db.Staffs.Find(StaffId);

            if (SelectedPharmacy == null || SelectedStaff == null)
            {
                return NotFound();
            }


            SelectedPharmacy.Staffs.Add(SelectedStaff);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Unassociates a chosen staff member with a specific pharmacy
        /// </summary>
        /// <param name="PharmacyID">Pharmacy Primary Key</param>
        /// <param name="StaffId">Staff Primary Key</param>
        /// <returns>
        /// CONTENT: A removed bridge between a staff member and pharmacy
        /// </returns>
        /// <example>
        /// POST api/PharmacyData/UnAssociatePharmacyWithStaff/8/3
        /// </example>
        [HttpPost]
        [Route("api/PharmacyData/UnAssociatePharmacyWithStaff/{PharmacyID}/{StaffId}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UnAssociatePharmacyWithStaff(int PharmacyID, int StaffId)
        {

            Pharmacy SelectedPharmacy = db.Pharmacies.Include(p => p.Staffs).Where(p => p.PharmacyID == PharmacyID).FirstOrDefault();
            Staff SelectedStaff = db.Staffs.Find(StaffId);

            if (SelectedPharmacy == null || SelectedStaff == null)
            {
                return NotFound();
            }


            SelectedPharmacy.Staffs.Remove(SelectedStaff);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Receives pharmacy picture data, uploads it to the webserver and updates the pharmacies HasPic option
        /// </summary>
        /// <param name="id">Pharmacy Primary Key</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F pharmacypic=@file.jpg "https://localhost:xx/api/PharmacyData/UploadPharmacyPic/8"
        /// HEADER: enctype=multipart/form-data
        /// Content: input image from form
        /// </example>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("api/pharmacydata/uploadpharmacypic/{id}")]
        public IHttpActionResult UploadPharmacyPic(int id)
        {

            bool haspic = false;
            string picextension;

            if (Request.Content.IsMimeMultipartContent())
            {

                int numfiles = HttpContext.Current.Request.Files.Count;

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var PharmacyPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (PharmacyPic.ContentLength > 0)
                    {
                        //establish valid file types
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(PharmacyPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Images/Pharmacies/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Pharmacies/"), fn);

                                //save the file
                                PharmacyPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the pharmacy haspic and picextension fields in the database
                                Pharmacy SelectedPharmacy = db.Pharmacies.Find(id);
                                SelectedPharmacy.PharmacyHasPic = haspic;
                                SelectedPharmacy.PicExtension = extension;
                                db.Entry(SelectedPharmacy).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Pharmacy Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
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

        private bool PharmacyExists(int id)
        {
            return db.Pharmacies.Count(e => e.PharmacyID == id) > 0;
        }
    }
}
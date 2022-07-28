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
                PharmacyDelivery = ph.PharmacyDelivery,
                PharmacyHasPic = ph.PharmacyHasPic,
                PicExtension = ph.PicExtension
            }));

            return Ok(PharmaciesDtos);
        }

        // GET: api/PharmacyData/5
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

        // PUT: api/PharmacyData/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/PharmacyData
        [ResponseType(typeof(Pharmacy))]
        [Route("api/pharmacydata/addpharmacy")]
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
        [HttpPost]
        [Route("api/pharmacydata/deletepharmacy/{id}")]
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

        [HttpPost]
        [Route("api/PharmacyData/AssociatePharmacyWithStaff/{PharmacyID}/{StaffId}")]
        public IHttpActionResult AssociateAnimalWithKeeper(int PharmacyID, int StaffId)
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

        [HttpPost]
        [Route("api/PharmacyData/UnAssociatePharmacyWithStaff/{PharmacyID}/{StaffId}")]
        public IHttpActionResult UnAssociateAnimalWithKeeper(int PharmacyID, int StaffId)
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


        [HttpPost]
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
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(PharmacyPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/animals/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Pharmacies/"), fn);

                                //save the file
                                PharmacyPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the animal haspic and picextension fields in the database
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
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
    public class StaffDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StaffData
        //Data controller endpoint to retrieve list of users
        [HttpGet]
        [Route("api/StaffData/ListStaffs")]
        [ResponseType(typeof(StaffDto))]
        public IHttpActionResult GetStaffs()
        {
            List<Staff> Staffs = db.Staffs.ToList();
            List<StaffDto> StaffsDtos = new List<StaffDto>();

            Staffs.ForEach(st => StaffsDtos.Add(new StaffDto()
            {
                StaffId = st.StaffId,
                FirstName = st.FirstName,
                LastName = st.LastName,
                Title = st.Title,
                Image = st.Image,
                DepartmentName = st.Department.DepartmentName,
            }));

            return Ok(StaffsDtos);
        }

        // GET: api/StaffData/5
        //Data controller endpoint to retrieve a user detail by specifying its id
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        [Route("api/staffdata/findstaff/{id}")]
        public IHttpActionResult FindStaff(int id)
        {
            Staff Staff = db.Staffs.Find(id);
            StaffDto StaffDto = new StaffDto()
            {
                StaffId = Staff.StaffId,
                FirstName = Staff.FirstName,
                LastName = Staff.LastName,
                Title = Staff.Title,
                Image = Staff.Image,
                DepartmentName = Staff.Department.DepartmentName,
            };

            if (Staff == null)
            {
                return NotFound();
            }

            return Ok(StaffDto);
        }

        // PUT: api/StaffData/5
        //Data controller endpoint to update a user details
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/staffdata/updatestaff/{id}")]
        public IHttpActionResult PutStaff(int id, Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staff.StaffId)
            {
                return BadRequest();
            }

            db.Entry(staff).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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

        // POST: api/StaffData
        //Data controller endpoint to create a new user
        [ResponseType(typeof(Staff))]
        [Route("api/stafftdata/addstaff")]
        public IHttpActionResult PostStaff(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Staffs.Add(staff);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = staff.StaffId }, staff);
        }

        // DELETE: api/StaffData/5
        //Data controller endpoint to delete a user
        [ResponseType(typeof(Staff))]
        public IHttpActionResult DeleteStaff(int id)
        {
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            db.Staffs.Remove(staff);
            db.SaveChanges();

            return Ok(staff);
        }

        //Get a collection of staff attched to a particular specified pharmacy
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        [Route("api/staffdata/liststaffforpharmacy/{id}")]
        public IHttpActionResult ListStaffForPharmacy(int id)
        {
            List<Staff> Staffs = db.Staffs.Where(
                s => s.Pharmacies.Any(
                    p => p.PharmacyID == id)
                ).ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();

            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                StaffId = s.StaffId,
                FirstName = s.FirstName,
                LastName = s.LastName
            }));

            return Ok(StaffDtos);
        }

        //Get a collection of staff not attched to a particular specified pharmacy
        [HttpGet]
        [ResponseType(typeof(StaffDto))]
        [Route("api/staffdata/liststaffnotatpharmacy/{id}")]
        public IHttpActionResult ListStaffNotAtPharmacy(int id)
        {
            List<Staff> Staffs = db.Staffs.Where(
                s => !s.Pharmacies.Any(
                    p => p.PharmacyID == id)
                ).ToList();
            List<StaffDto> StaffDtos = new List<StaffDto>();

            Staffs.ForEach(s => StaffDtos.Add(new StaffDto()
            {
                StaffId = s.StaffId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Title = s.Title,
            }));

            return Ok(StaffDtos);
        }

        /// <summary>
        /// dispose db connection resource
        /// </summary>
        /// <param name="disposing">boolean flag</param>
        /// <returns>
        /// <example>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// check if an staff with a sepcified id exists
        /// </summary>
        /// <param name="id">Staff Primary Key</param>
        /// <returns>
        /// <example>
        private bool StaffExists(int id)
        {
            return db.Staffs.Count(e => e.StaffId == id) > 0;
        }
    }
}
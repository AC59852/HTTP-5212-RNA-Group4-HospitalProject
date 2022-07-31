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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentData/listdepartments
        /// <summary>
        /// Returns a list of department in the system
        /// </summary>
        /// <returns>
        /// </returns>
        [HttpGet]
        [Route("api/DepartmentData/listdepartments")]
        public IEnumerable<DepartmentDto> ListDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(p => DepartmentDtos.Add(new DepartmentDto()
            {
                DepartmentID = p.DepartmentID,
                DepartmentName = p.DepartmentName,
               
            }));

            return DepartmentDtos;
        }


        // GET: api/DepartmentData/findDepartment/5
        /// <summary>
        /// Returns a page a department based on id in the system
        /// </summary>
        /// <returns>
        /// </returns>
        [ResponseType(typeof(Department))]
        [HttpGet]
        [Route("api/Departmentdata/findDepartment/{id}")]
        public IHttpActionResult FindDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            DepartmentDto DepartmentDto = new DepartmentDto()
            {
                DepartmentID = Department.DepartmentID,
                DepartmentName = Department.DepartmentName,
               
            };

            if (Department == null)
            {
                return NotFound();
            }

            return Ok(DepartmentDto);
        }


        // POST: api/DepartmentData/updateDepartment/5
        /// <summary>
        ///Updates a department in the system
        /// </summary>
        /// <returns>
        /// </returns>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/DepartmentData/updateDepartment/{id}")]
        public IHttpActionResult updateDepartment(int id, Department Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Department.DepartmentID)
            {
                return BadRequest();
            }

            db.Entry(Department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/DepartmentData/AddDepartment
        /// <summary>
        /// Adds a new department in the system
        /// </summary>
        /// <returns>
        /// </returns>
        [ResponseType(typeof(Department))]
        [HttpPost]
        [Route("api/DepartmentData/AddDepartment")]
        public IHttpActionResult AddDepartment(Department Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Debug.WriteLine("am there");

            Debug.WriteLine(Department);
            db.Departments.Add(Department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Department.DepartmentID }, Department);
        }

        // DELETE: api/DepartmentData/DeleteDepartment/5
        /// <summary>
        /// deletes a department in the system
        /// </summary>
        /// <returns>
        /// </returns>
        [ResponseType(typeof(Department))]
        [HttpPost]
        [Route("api/departmentdata/deletedepartment/{id}")]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            if (Department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(Department);
            db.SaveChanges();

            return Ok(Department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// checks if a department exists in the system
        /// </summary>
        /// <returns>
        /// </returns>
        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentID == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

       
        // A staff belongs to a department
        // An department can have one or more staffs

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public ICollection<Pharmacy> Pharmacies { get; set; }
    }

    public class StaffDto
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Department { get; set; }
    }
}
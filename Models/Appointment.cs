using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PatientNotes { get; set; }

        // A staff can have many appointments
        // An appointment can be fulfilled by one staff

        [ForeignKey("Staff")]
        public int StaffID { get; set; }
        public virtual Staff Staff { get; set; }
    }
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string StaffName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PatientNotes { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; }
        public string PatientName { get; set; }
        public string PrescriptionDrug { get; set; }
        public int PrescriptionRefills { get; set; }
        public string PrescriptionDosage { get; set; }
        public string PrescriptionInstructions { get; set; }

        // A pharmacy can have many prescriptions
        // A prescription can only be filled at one pharmacy
        [ForeignKey("Pharmacy")]
        public int PharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }

        [ForeignKey("Staff")]
        public int? StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public virtual Staff Staff { get; set; }
    }

    public class PrescriptionDto
    {
        public int PrescriptionID { get; set; }
        public string PatientName { get; set; }
        public string PrescriptionDrug { get; set; }
        public int PrescriptionRefills { get; set; }
        public string PrescriptionDosage { get; set; }
        public string PrescriptionInstructions { get; set; }

        public int PharmacyID { get; set; }
        public string PharmacyName { get; set; }

        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}
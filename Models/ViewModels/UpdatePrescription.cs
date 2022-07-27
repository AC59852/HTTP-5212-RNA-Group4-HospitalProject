using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class UpdatePrescription
    {
        public PrescriptionDto SelectedPrescription { get; set; }

        public IEnumerable<PharmacyDto> PharmacyOptions { get; set; }

        public IEnumerable<StaffDto> StaffOptions { get; set; }
    }
}
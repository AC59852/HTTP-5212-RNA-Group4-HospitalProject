using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class DetailsPharmacy
    {
        public PharmacyDto SelectedPharmacy { get; set; }
        public IEnumerable<PrescriptionDto> RelatedPrescriptions { get; set; }

        public IEnumerable<StaffDto> RelatedStaff { get; set; }

        public IEnumerable<StaffDto> AvailableStaff { get; set; }
    }
}
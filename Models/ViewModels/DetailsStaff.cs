using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class DetailsStaff
    {
        public StaffDto SelectedStaff { get; set; }

        public IEnumerable<PharmacyDto> AvailablePharmacies { get; set; }
    }
}
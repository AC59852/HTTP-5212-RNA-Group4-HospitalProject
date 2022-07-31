using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class DetailsResearch
    {
        public ResearchDto SelectedResearch { get; set; } 
        public IEnumerable<StaffDto> RelatedStaff { get; set; }
        public IEnumerable<DonarDto> RelatedDonars { get; set; }
    }
}
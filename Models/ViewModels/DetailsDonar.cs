using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class DetailsDonar
    {
        public DonarDto SelectedDonar { get; set; }
        public IEnumerable<ResearchDto> RelatedResearches { get; set; }
        
    }
}
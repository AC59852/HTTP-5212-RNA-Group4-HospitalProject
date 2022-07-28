using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class EditHUpdate
    {
        public HUpdateDto editHUpdate { get; set; }
        public IEnumerable<Department> editDepartments { get; set; }
    }
}
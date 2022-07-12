using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Pharmacy
    {
        public int PharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyLocation { get; set; }
        public int PharmacyWaitTime { get; set; }
        public int PharmacyOpenTime { get; set; }
        public int PharmacyCloseTime { get; set; }
        public bool PharmacyDelivery { get; set; }

        public ICollection<Staff> Staffs { get; set; }

    }

    public class PharmacyDto
    {
        public int PharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyLocation { get; set; }
        public int PharmacyWaitTime { get; set; }
        public int PharmacyOpenTime { get; set; }
        public int PharmacyCloseTime { get; set; }
        public bool PharmacyDelivery { get; set; }
    }
}
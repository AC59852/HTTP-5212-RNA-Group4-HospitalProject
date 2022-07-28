using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    // HUpdate : Hospital Update
    public class HUpdate
    {
        [Key]
        public int HUpdateId { get; set; }
        public string HUpdateTitle { get; set; }
        public string HUpdateDesc { get; set; }
        public DateTime HUpdateDate { get; set; }

        // creating HUpdateType which will have value "1:update" or "2:event"
        // Use? 
        // In Hospital there can be "Update" like "A.C maintenance" 
        // An Hospital can organize "Event" like "staff meeting at Toronto Islands for social event"
        public int HUpdateType { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    // HUpdate : Hospital Update

    // creating enum which will have value "update" or "event"
    // Use? 
    // In Hospital there can be "Update" like "A.C maintenance" 
    // An Hospital can organize "Event" like "staff meeting at Toronto Islands for social event"
    public enum HUpdateTags
    {
        Update = 0,
        Event = 1,
    }
    public class HUpdate
    {
        [Key]
        public int HUpdateId { get; set; }
        public string HUpdateTitle { get; set; }
        public string HUpdateDesc { get; set; }
        public DateTime HUpdateDate { get; set; }
        public virtual int HUpdateTagId { get; set; }

    }
    public class HUpdateDto
    {
        public int HUpdateId { get; set; }
        public string HUpdateTitle { get; set; }
        public string HUpdateDesc { get; set; }
        public DateTime HUpdateDate { get; set; }
        public int HUpdateTagId { get; set; }

        [EnumDataType(typeof(HUpdateTags))]
        public HUpdateTags HUpdateTag
        {
            get
            {
                return (HUpdateTags)this.HUpdateTagId;
            }
            set
            {
                this.HUpdateTagId = (int)value;
            }
        }

    }
}
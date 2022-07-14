using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }

        [ForeignKey("HUpdate")]
        public int HUpdateId { get; set; }
        public virtual HUpdate HUpdate { get; set; }
    }

    public class ArticleDto
    {
        public int ArticleID { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public int HUpdateId { get; set; }
        public string HUpdateTitle { get; set; }
    }
}
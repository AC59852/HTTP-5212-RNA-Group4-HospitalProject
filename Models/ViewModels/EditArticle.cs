using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class EditArticle
    {
        public ArticleDto editArticle { get; set; }
        public IEnumerable<HUpdate> editHUpdates { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class DetailsHUpdate
    {
        public HUpdate SelectedUpdate { get; set; }
        public IEnumerable<ArticleDto> ArticlesForHUpdate  { get; set; }
    }
}
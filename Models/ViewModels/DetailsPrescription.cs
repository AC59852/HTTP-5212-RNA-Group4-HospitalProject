﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models.ViewModels
{
    public class DetailsPrescription
    {
        public bool IsAdmin { get; set; }
        public PrescriptionDto SelectedPrescription { get; set; }
    }
}
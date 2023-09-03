using System;
using System.Collections.Generic;

namespace App.Core.Models
{
    public class VacancyModel : BaseLookupModel
    {
        public int MaxApplicant { get; set; }
        public DateTime ExpiryDate { get; set; }

        public List<ApplicantVacancyModel> ApplicantVacancy { get; set; }
    }
}

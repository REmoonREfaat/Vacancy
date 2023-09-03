using App.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Entities
{
    public class Vacancy : BaseLookupEntity
    {
        public int MaxApplicant { get; set; }
        public DateTime ExpiryDate { get; set; }
        public ICollection<ApplicantVacancy> ApplicantVacancy { get; set; }

    }
}

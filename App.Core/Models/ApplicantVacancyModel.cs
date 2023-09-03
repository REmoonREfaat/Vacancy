using System;

namespace App.Core.Models
{
    public class ApplicantVacancyModel : BaseModel
    {
        //public VacancyModel Vacancy { get; set; }
        public long VacancyId { get; set; }
        public string VacancyName { get; set; }
        public string VacancyNameAr { get; set; }


        public string ApplicantId { get; set; }
        //public AppUserModel Applicant { get; set; }
        public string FullName { get; set; }

    }
}

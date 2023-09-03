using System;

namespace App.API.DTOs
{
    public class ApplicantVacancyDTO : BaseDTO
    {

        public long VacancyId { get; set; }
        //public VacancyDTO Vacancy { get; set; }

        public string ApplicantId { get; set; }
        //public AppUserDTO Applicant { get; set; }
        public string FullName { get; set; }

    }
}

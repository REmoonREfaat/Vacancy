using App.Core.Entities.Base;

namespace App.Core.Entities
{
    public class ApplicantVacancy : BaseEntity
    {
        public long VacancyId{ get; set; }
        public Vacancy Vacancy { get; set; }

        public string ApplicantId { get; set; }
        public AppUser Applicant { get; set; }
    }
}

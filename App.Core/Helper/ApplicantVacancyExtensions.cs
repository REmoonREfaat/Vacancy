using App.Core.Entities;
using App.Core.Models;
using System.Linq;

namespace App.Core.Helper
{
    public static class ApplicantVacancyExtensions
    {

        public static IQueryable<ApplicantVacancyModel> GetApplicantVacanciesModel(this IQueryable<ApplicantVacancy> entities)
        {
            var Models = entities.Select(x => x.GetApplicantVacancyModel());
            return Models;
        }


        public static ApplicantVacancyModel GetApplicantVacancyModel(this ApplicantVacancy entity)
        {
            var Model = new ApplicantVacancyModel
            {
                Id = entity.Id,
                CreationDate = entity.CreationDate,

                ApplicantId = entity.ApplicantId,
                FullName = entity.Applicant.FirstName + " " + entity.Applicant.LastName,

                VacancyId = entity.VacancyId,
                //VacancyName = entity.Vacancy.Name,
                //VacancyNameAr = entity.Vacancy.NameAr

            };
            return Model;
        }


    }
}

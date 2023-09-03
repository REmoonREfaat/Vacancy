using App.Core.Entities;
using System.Linq;
using App.Core.Models;
using System.Collections.Generic;

namespace App.Core.Helper
{
    public static class VacancyExtensions
    {

        public static IQueryable<VacancyModel> GetVacanciesModel(this IQueryable<Vacancy> entities)
        {
            var Models = entities.Select(x => x.GetVacancyModel());
            return Models;
        }


        public static VacancyModel GetVacancyModel(this Vacancy entity)
        {
            var Model = new VacancyModel
            {
                Id = entity.Id,
                Name = entity.Name,
                NameAr = entity.NameAr,
                CreationDate = entity.CreationDate,
                Description = entity.Description,
                DescriptionAr = entity.DescriptionAr,
                ExpiryDate = entity.ExpiryDate,
                MaxApplicant = entity.MaxApplicant,

            };
            return Model;

        }

        public static int GetApplicantCount(this Vacancy entity)
        {
            if (!entity.ApplicantVacancy.Any())
                return 0;
            else
                return entity.ApplicantVacancy.Count();
        }

        public static int GetApplicantCount(this VacancyModel model)
        {
            if (!model.ApplicantVacancy.Any())
                return 0;
            else
                return model.ApplicantVacancy.Count();
        }
    }
}

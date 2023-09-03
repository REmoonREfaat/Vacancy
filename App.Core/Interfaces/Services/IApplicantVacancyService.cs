using App.Core.Entities;
using App.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Services
{
    public interface IApplicantVacancyService : IGenericService<ApplicantVacancy>
    {

        Task<IQueryable<ApplicantVacancyModel>> GetAllApplicantVacancies();
        Task<IQueryable<ApplicantVacancyModel>> GetAllApplicantByVacancyId(long VacancyId);
        Task<ApplicantVacancyModel> GetApplicantVacancyById(long id);
        Task<ApplicantVacancyModel> AddApplicantVacancy(ApplicantVacancyModel model);
        Task<ApplicantVacancyModel> UpdateApplicantVacancy(ApplicantVacancyModel model);
        Task<int> ApplicantCountByVacancyId(long VacancyId);
        Task<DateTime> GetLastApplicantVacancy(string ApplicantId);
        Task<int> GetSameApplicantVacancy(string ApplicantId, long VacancyId);
    }
}

using AutoMapper;
using App.Common.Services.Logger;
using App.Core.Entities;
using App.Core.Interfaces.Repository;
using App.Core.Interfaces.Services;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Helper;
using System;

namespace App.Core.Services
{
    public class ApplicantVacancyService : GenericService<ApplicantVacancy>, IApplicantVacancyService
    {

        public ApplicantVacancyService(
            IGenericRepository<ApplicantVacancy> oRepository,
            Ilogger logger,
            IMapper mapper)
            : base(oRepository, logger, mapper)
        {
        }

        public async Task<IQueryable<ApplicantVacancyModel>> GetAllApplicantVacancies()
        {
            var (Result, TotalItems) = await GetAll<ApplicantVacancy>("id", null, null, null, /*x => x.Vacancy,*/ y => y.Applicant);
            var models = Result.GetApplicantVacanciesModel();
            return models;
        }


        public async Task<IQueryable<ApplicantVacancyModel>> GetAllApplicantByVacancyId(long VacancyId)
        {
            var (Result, TotalItems) = await GetAll<ApplicantVacancy>("id", x => x.VacancyId == VacancyId, null, null, /*x => x.Vacancy,*/ y => y.Applicant);
            var Models = Result.GetApplicantVacanciesModel();
            return Models;
        }

        public async Task<ApplicantVacancyModel> GetApplicantVacancyById(long id)
        {
            var entity = await GetById<ApplicantVacancy>(id, x => x.Vacancy, y => y.Applicant);
            var Model = entity.GetApplicantVacancyModel();
            return Model;
        }

        public async Task<ApplicantVacancyModel> AddApplicantVacancy(ApplicantVacancyModel model)
        {
            var entity = mapper.Map<ApplicantVacancy>(model);
            var result = await Add(entity);
            var Model = mapper.Map<ApplicantVacancyModel>(result);
            return Model;
        }

        public async Task<ApplicantVacancyModel> UpdateApplicantVacancy(ApplicantVacancyModel model)
        {
            var entity = mapper.Map<ApplicantVacancy>(model);
            var result = await Update(entity.Id, entity);
            var Model = mapper.Map<ApplicantVacancyModel>(result);
            return Model;
        }

        public async Task<int> ApplicantCountByVacancyId(long VacancyId)
        {
            var (Result, TotalItems) = await GetAll<ApplicantVacancy>("id", x => x.VacancyId == VacancyId, null, null, null);
            return TotalItems;
        }


        public async Task<DateTime> GetLastApplicantVacancy(string ApplicantId)
        {
            var CreationDate = DateTime.Now.AddDays(-7);
            var (Result, TotalItems) = await GetAll<ApplicantVacancy>("CreationDate", x => x.ApplicantId == ApplicantId,  null, null, null);
            if (Result.LastOrDefault() != null)
            CreationDate = Result.LastOrDefault().CreationDate;
            return CreationDate;
        }

        public async Task<int> GetSameApplicantVacancy(string ApplicantId, long VacancyId)
        {

            var (Result, TotalItems) = await GetAll<ApplicantVacancy>("CreationDate", x => x.ApplicantId == ApplicantId && x.VacancyId == VacancyId, null, null, null);
            return TotalItems;
        }
    }
}

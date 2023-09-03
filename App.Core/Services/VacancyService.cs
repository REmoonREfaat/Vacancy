using App.Common.Services.Logger;
using App.Core.Entities;
using App.Core.Helper;
using App.Core.Interfaces.Repository;
using App.Core.Interfaces.Services;
using App.Core.Models;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class VacancyService : GenericService<Vacancy>, IVacancyService
    {
        public VacancyService(IGenericRepository<Vacancy> oRepository, Ilogger logger, IMapper mapper)
            : base(oRepository, logger, mapper)
        {
        }

        public async Task<IQueryable<VacancyModel>> GetAllVacancies()
        {
            var (Result, TotalItems) = await GetAll<Vacancy>("id");
            var Models = Result.GetVacanciesModel();
            return Models;
        }
        public async Task<IQueryable<VacancyModel>> SearchByVacancyName(string Search)
        {
            var (Result, TotalItems) = await GetAll<Vacancy>("id", x => x.Name.Contains(Search) || x.NameAr.Contains(Search));
            var Models = Result.GetVacanciesModel();
            return Models;
        }

        public async Task<VacancyModel> GetVacancyById(long id)
        {
            var Entity = await GetById<VacancyModel>(id);
            var Model = Entity.GetVacancyModel();
            return Model;
        }

        public async Task<VacancyModel> AddVacancy(VacancyModel model)
        {
            var NewEntity = mapper.Map<Vacancy>(model);
            var Result = await Add(NewEntity);
            var Model = mapper.Map<VacancyModel>(Result);
            return Model;
        }

        public async Task<VacancyModel> UpdateVacancy(VacancyModel model)
        {
            var Entity = mapper.Map<Vacancy>(model);
            var Result = await Update(Entity.Id, Entity);
            var Model = mapper.Map<VacancyModel>(Result);
            return Model;
        }

        public async Task<int> GetMaxApplicantPerVacancyById(long id)
        {
            var Entity = await GetById<VacancyModel>(id);
            return Entity.MaxApplicant;
        }


    }
}

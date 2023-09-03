using App.Core.Entities;
using App.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Interfaces.Services
{
    public interface IVacancyService : IGenericService<Vacancy>
    {

        Task<IQueryable<VacancyModel>> GetAllVacancies();
        Task<IQueryable<VacancyModel>> SearchByVacancyName(string Search);
        Task<VacancyModel> GetVacancyById(long id);
        Task<VacancyModel> AddVacancy(VacancyModel model);
        Task<VacancyModel> UpdateVacancy(VacancyModel model);
        Task<int> GetMaxApplicantPerVacancyById(long id);



    }
}

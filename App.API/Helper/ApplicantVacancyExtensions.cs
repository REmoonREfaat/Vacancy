using App.API.DTOs;
using App.Common.Services.Logger;
using App.Core.Interfaces.Services;
using App.Core.Models;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace App.API.Helper
{

    public static class ApplicantVacancyExtensions
    {

        internal static async Task<ResponseModel<ApplicantVacancyDTO>> CheckAndAddApplicantVacancyAsync(IApplicantVacancyService service, IVacancyService vacancyService, string userId, long vacancyId, IMapper mapper, Ilogger logger)
        {
            try
            {
                int maxApplicantPerVacancy = await vacancyService.GetMaxApplicantPerVacancyById(vacancyId);
                int applicantCountByVacancyId = await service.ApplicantCountByVacancyId(vacancyId);

                if (maxApplicantPerVacancy >= applicantCountByVacancyId)
                    return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, true, "Max Applicant Exceed");

                var creationDate = await service.GetLastApplicantVacancy(userId);
                var yesterday = DateTime.Now.AddDays(-1);
                if (creationDate >= yesterday)
                    return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, true, "Max Daily Applied Vacancy Exceed");

                if (await service.GetSameApplicantVacancy(userId, vacancyId) >= 1)
                    return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, true, "This Vacancy is Applied Before");

                var model = new ApplicantVacancyDTO(); // Create your DTO model here.
                var applicantVacancyModel = mapper.Map<ApplicantVacancyModel>(model);
                applicantVacancyModel.ApplicantId = userId;
                var result = await service.AddApplicantVacancy(applicantVacancyModel);
                var applicantVacancyDTO = mapper.Map<ApplicantVacancyDTO>(result);
                var responseModel = HelperClass<ApplicantVacancyDTO>.CreateResponseModel(applicantVacancyDTO, false, "");
                return responseModel;
            }
            catch (Exception ex)
            {
                logger.Error("Error occurred in CheckAndAddApplicantVacancyAsync: " + ex.Message);
                return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, true, ex.Message);
            }
        }
    }
}

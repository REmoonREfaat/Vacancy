using App.API.DTOs;
using App.API.Helper;
using App.Common.Services.Logger;
using App.Core.Interfaces.Services;
using App.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    public class ApplicantVacancyController : ControllerBase
    {
        private readonly IMapper _mapper;
        protected readonly Ilogger _logger;
        private readonly IApplicantVacancyService _service;
        private readonly IVacancyService _vacancyService;

        public ApplicantVacancyController(Ilogger logger, IMapper mapper, IApplicantVacancyService service, IVacancyService vacancyService)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
            _vacancyService = vacancyService;
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<List<ApplicantVacancyDTO>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllApplicantVacancies();
                var list = _mapper.Map<List<ApplicantVacancyDTO>>(result.ToList());
                var responseModel = HelperClass<List<ApplicantVacancyDTO>>.CreateResponseModel(list, false, "");
                return responseModel;

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured ApplicantVacancyController\\GetAll" + " with EX: " + ex.ToString());
                return HelperClass<List<ApplicantVacancyDTO>>.CreateResponseModel(null, true, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<List<ApplicantVacancyDTO>>> GetAllApplicantByVacancyId(long Id)
        {
            try
            {
                var result = await _service.GetAllApplicantByVacancyId(Id);
                var list = _mapper.Map<List<ApplicantVacancyDTO>>(result.ToList());
                var responseModel = HelperClass<List<ApplicantVacancyDTO>>.CreateResponseModel(list, false, "");
                return responseModel;

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured ApplicantVacancyController\\GetAll" + " with EX: " + ex.ToString());
                return HelperClass<List<ApplicantVacancyDTO>>.CreateResponseModel(null, true, ex.Message);
            }
        }


        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<ApplicantVacancyDTO>> GetById(long Id)
        {

            try
            {
                var result = await _service.GetApplicantVacancyById(Id);
                var ApplicantVacancyDTO = _mapper.Map<ApplicantVacancyDTO>(result);
                var ApplicantVacancy = HelperClass<ApplicantVacancyDTO>.CreateResponseModel(ApplicantVacancyDTO, false, "");
                return ApplicantVacancy;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured ApplicantVacancyController\\GetById" + Id + " with EX: " + ex.ToString());
                return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, true, ex.Message);
            }
        }


        /// <summary>
        /// Authorized API 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Applicant")]
        public async Task<ResponseModel<ApplicantVacancyDTO>> AddApplicantVacancy([FromBody] ApplicantVacancyDTO model)
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await ApplicantVacancyExtensions.CheckAndAddApplicantVacancyAsync(
                _service,                _vacancyService,                userId,                model.VacancyId,
                _mapper,
                _logger);

            return result;

        }


        /// <summary>
        /// Authorized API 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [HttpPost, ActionName("DeleteApplicantVacancy")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Applicant")]
        public async Task<ResponseModel<ApplicantVacancyDTO>> Delete(long Id)
        {
            try
            {
                await _service.ChangeStatus(Id, false);
                return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, false, "Deleted !");

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured BrandController\\DeleteApplicantVacancy" + Id + " with EX: " + ex.ToString());
                return HelperClass<ApplicantVacancyDTO>.CreateResponseModel(null, true, "Error In Deleting");

            }
        }
    }
}
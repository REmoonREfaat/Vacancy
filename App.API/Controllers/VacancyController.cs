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
    public class VacancyController : ControllerBase
    {
        private readonly IMapper _mapper;
        protected readonly Ilogger _logger;
        private readonly IVacancyService _service;

        public VacancyController(Ilogger logger, IMapper mapper, IVacancyService service)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer, Applicant")]
        public async Task<ResponseModel<List<VacancyDTO>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllVacancies();
                var list = _mapper.Map<List<VacancyDTO>>(result.ToList());
                var responseModel = HelperClass<List<VacancyDTO>>.CreateResponseModel(list, false, "");
                return responseModel;

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured VacancyController\\GetAll" + " with EX: " + ex.ToString());
                return HelperClass<List<VacancyDTO>>.CreateResponseModel(null, true, ex.Message);
            }
        }
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer, Applicant")]
        public async Task<ResponseModel<List<VacancyDTO>>> SearchByVacancyName(string Search)
        {
            try
            {
                var result = await _service.SearchByVacancyName(Search);
                var list = _mapper.Map<List<VacancyDTO>>(result.ToList());
                var responseModel = HelperClass<List<VacancyDTO>>.CreateResponseModel(list, false, "");
                return responseModel;

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured VacancyController\\GetAll" + " with EX: " + ex.ToString());
                return HelperClass<List<VacancyDTO>>.CreateResponseModel(null, true, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer, Applicant")]
        public async Task<ResponseModel<VacancyDTO>> GetById(long Id)
        {

            try
            {
                var result = await _service.GetVacancyById(Id);
                var VacancyDTO = _mapper.Map<VacancyDTO>(result);
                var Vacancy = HelperClass<VacancyDTO>.CreateResponseModel(VacancyDTO, false, "");
                return Vacancy;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured VacancyController\\GetById" + Id + " with EX: " + ex.ToString());
                return HelperClass<VacancyDTO>.CreateResponseModel(null, true, ex.Message);
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
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<VacancyDTO>> AddVacancy([FromBody] VacancyDTO model)
        {
            try
            {
                //string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var VacancyModel = _mapper.Map<VacancyModel>(model);
                //VacancyModel.CreatedBy = id;
                var result = await _service.AddVacancy(VacancyModel);
                var VacancyDTO = _mapper.Map<VacancyDTO>(result);
                var ResponseModel = HelperClass<VacancyDTO>.CreateResponseModel(VacancyDTO, false, "");
                return ResponseModel;
            }

            catch (Exception ex)
            {
                _logger.Error("Error occured VacancyController\\Add" + " with EX: " + ex.Message);
                return HelperClass<VacancyDTO>.CreateResponseModel(null, true, ex.Message);
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
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<VacancyDTO>> UpdateVacancy([FromBody] VacancyDTO model)
        {
            try
            {
                //string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var VacancyModel = _mapper.Map<VacancyModel>(model);
                //VacancyModel.UserId = id;
                var Model = await _service.UpdateVacancy(VacancyModel);
                var VacancyDTO = _mapper.Map<VacancyDTO>(Model);
                var ResponseModel = HelperClass<VacancyDTO>.CreateResponseModel(VacancyDTO, false, "");
                return ResponseModel;
            }

            catch (Exception ex)
            {
                _logger.Error("Error occured VacancyController\\UpdateVacancy" + " with EX: " + ex.Message);
                return HelperClass<VacancyDTO>.CreateResponseModel(null, true, ex.Message);
            }

        }


        /// <summary>
        /// Authorized API 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [HttpPost, ActionName("DeleteVacancy")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<VacancyDTO>> Delete(long Id)
        {
            try
            {
                await _service.ChangeStatus(Id, false);
                return HelperClass<VacancyDTO>.CreateResponseModel(null, false, "Deleted !");

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured BrandController\\DeleteVacancy" + Id + " with EX: " + ex.ToString());
                return HelperClass<VacancyDTO>.CreateResponseModel(null, true,"Error In Deleting");

            }
        }


        /// <summary>
        /// Authorized API 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [HttpPost, ActionName("DeactivateVacancy")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "SuperAdmin, Employer")]
        public async Task<ResponseModel<VacancyDTO>> Deactivate(long Id)
        {
            try
            {
                var VacancyModel = _service.GetVacancyById(Id).Result;
                VacancyModel.RecordStatus = Core.Entities.Base.RecordStatus.Deactivated;
                var Model = await _service.UpdateVacancy(VacancyModel);

                return HelperClass<VacancyDTO>.CreateResponseModel(null, false, "Deactivate !");

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured BrandController\\DeactivateVacancy" + Id + " with EX: " + ex.ToString());
                return HelperClass<VacancyDTO>.CreateResponseModel(null, true, "Error In Deactivating");

            }
        }
    }
}
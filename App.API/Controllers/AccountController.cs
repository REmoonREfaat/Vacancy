using App.API.DTOs;
using App.API.Helper;
using App.Common.Services.Logger;
using App.Core.Entities;
using App.Core.Identity;
using App.Core.Interfaces.Services;
using App.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        protected readonly Ilogger _logger;
        protected readonly IAppUserManagerService _appUserManagerService;
        public AccountController(Ilogger logger, IMapper mapper,
             ApplicationUserManager userManager,
             SignInManager<AppUser> signInManager,
            IAppUserManagerService appUserManagerService,
             IConfiguration config)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _appUserManagerService = appUserManagerService;

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/[controller]/[action]")]
        public async Task<ResponseModel<AppUserDTO>> Login([FromBody] LoginDTO model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return HelperClass<AppUserDTO>.CreateResponseModel(null, true,
                      string.Join(",", ModelState.Values
                      .SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)));

                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user == null)
                    _userManager.Users.Where(x => x.PhoneNumber == model.UserName).FirstOrDefault();

                if (user == null)
                    return new ResponseModel<AppUserDTO>()
                    {
                        IsError = true,
                        Timestamp = DateTime.Now,
                        Result = null,
                        Description = "Email not exist"
                    };

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                    return HelperClass<AppUserDTO>.CreateResponseModel(GetAppUserDTO(user), false, null);


                else if (result.IsNotAllowed)
                    return HelperClass<AppUserDTO>.CreateResponseModel(null, true, "Invalid Username or Password");

                else if (result.IsLockedOut)
                    return HelperClass<AppUserDTO>.CreateResponseModel(null, true,
                        "For security reasons your account is temporarily locked, try again in 15 mins");

                else
                    return HelperClass<AppUserDTO>.CreateResponseModel(null, true,
                      "Invalid Password");




            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountController\\Login" + " with EX: " + ex.ToString());
                return HelperClass<AppUserDTO>.CreateResponseModel(null, true,
                         ex.Message);



            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/[controller]/[action]")]
        public async Task<ResponseModel<BooleanResultDTO>> Registeration([FromBody] RegisterDTO registerModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true,
                      string.Join(",", ModelState.Values
                      .SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)));

                var newuser = new AppUserModel
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    Email = registerModel.Email,
                    UserName = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber,
                    EmailConfirmed = true,
                    Password = registerModel.Password,
                    Roles = registerModel.Roles,

                };
                return await _appUserManagerService.AddUser(newuser);

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountController\\Registeration" + " with EX: " + ex.ToString());
                return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true,
                       ex.Message);
            }

        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("api/[controller]/[action]")]
        public async Task<ResponseModel<BooleanResultDTO>> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true,
                      string.Join(",", ModelState.Values
                      .SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)));


                string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, "User not exist");


                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, result.Errors.ElementAt(0).Description);

                return HelperClass<BooleanResultDTO>.CreateResponseModel(new BooleanResultDTO()
                {
                    Success = true,
                }, false, "Password Changed Successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountController\\ChangePassword" + " with EX: " + ex.ToString());
                return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/[controller]/[action]")]
        public async Task<ResponseModel<BooleanResultDTO>> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true,
                    string.Join(",", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)));

                return await _appUserManagerService.ResetPassword(model.Email, model.Code, model.NewPassword);

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountManagement\\ResetPassword" + " with EX: " + ex.ToString());
                return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, ex.Message);
            }

        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("api/[controller]/[action]")]
        public async Task<ResponseModel<BooleanResultDTO>> EditProfile([FromBody] AppUserDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true,
                     string.Join(",", ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage)));

                string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, "Invalid User");

                var appUserModel = _mapper.Map<AppUserModel>(model);
                var result = await _appUserManagerService.UpdateUser(appUserModel);

                if (result.IsError)
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, result.Description);

                else
                    return HelperClass<BooleanResultDTO>.CreateResponseModel(new BooleanResultDTO() { Success = true },
                        false, "Account Info Changed Successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountController\\EditProfile" + " with EX: " + ex.ToString());
                return HelperClass<BooleanResultDTO>.CreateResponseModel(null, true, ex.Message);
            }

        }


        [HttpGet]
        [AllowAnonymous]
        [Route("api/[controller]/[action]")]
        public async Task<ResponseModel<AppUserDTO>> CheckEmailExist(string email)
        {
            try
            {
                var res = await _appUserManagerService.GetUserByEmail(email);
                if (res == null)
                    return HelperClass<AppUserDTO>.CreateResponseModel(null, true, "Email not exist");

                var appUserDTO = _mapper.Map<AppUserDTO>(res);
                return HelperClass<AppUserDTO>.CreateResponseModel(appUserDTO, false, null);

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountController\\CheckEmailExist" + email + " with EX: " + ex.ToString());
                return HelperClass<AppUserDTO>.CreateResponseModel(null, true, ex.Message);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/[controller]/[action]")]
        public ResponseModel<AppUserDTO> CheckPhonenumberExist(string phoneNumber)
        {
            try
            {
                var res = _appUserManagerService.GetUserByMobile(phoneNumber);
                if (res == null)
                    return HelperClass<AppUserDTO>.CreateResponseModel(null, true, "PhoneNumber not exist");

                var appUserDTO = _mapper.Map<AppUserDTO>(res);
                return HelperClass<AppUserDTO>.CreateResponseModel(appUserDTO, false, null);

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured AccountController\\CheckPhonenumberExist" + phoneNumber + " with EX: " + ex.ToString());
                return HelperClass<AppUserDTO>.CreateResponseModel(null, true, ex.Message);
            }

        }

        public AppUserDTO GetAppUserDTO(AppUserModel user)
        {
            var claims = new[]
                               {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
              _config["Tokens:Audience"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: creds);
            AppUserDTO appUserDTO = new AppUserDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                EmailConfirmed = user.EmailConfirmed,
                DateAdded = user.DateAdded

            };
            return appUserDTO;
        }

        public AppUserDTO GetAppUserDTO(AppUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;

            var rolesClaims = new List<Claim>();

            foreach (var role in roles)
                rolesClaims.Add(new Claim("roles", role));

            var claims = new[]
                               {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        }
            .Union(rolesClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
              _config["Tokens:Audience"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: creds);
            AppUserDTO appUserDTO = new AppUserDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                EmailConfirmed = user.EmailConfirmed,
                DateAdded = user.DateAdded

            };
            return appUserDTO;
        }
    }
}

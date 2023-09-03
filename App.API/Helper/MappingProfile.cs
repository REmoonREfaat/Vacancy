using AutoMapper;
using App.Core.Entities;
using App.Core.Entities.Base;
using App.Core.Models;
using System;
using System.Linq;
using System.Reflection;
using App.API.DTOs;

namespace App.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            var asm = Assembly.Load("App.Core");
            var classes =
                asm.GetTypes().Where(p =>
                    p.Namespace != null && (p.Namespace.Equals("App.Core.Entities") || p.Namespace.Equals("App.Core.Entities.Lookups")) &&
                    p.IsClass

                && (p.IsSubclassOf(typeof(BaseEntity)) ||
                    p.IsSubclassOf(typeof(BaseNameEntity)) ||
                    p.IsSubclassOf(typeof(BaseLookupEntity)))

                    && (p.IsSubclassOf(typeof(BaseEntity)) ||
                        p.IsSubclassOf(typeof(BaseNameEntity)) ||
                        p.IsSubclassOf(typeof(BaseLookupEntity)))


                ).ToList();
            foreach (Type c in classes)
            {
                CreateMap(c, c)
                        .ForMember("CreationDate", act => act.Ignore())
                        .ForMember("LastUpdatedDate", act => act.Ignore());
            }
            // map Entity with Model
            CreateMap<AppUser, AppUserModel>().ReverseMap();
            CreateMap<Vacancy, VacancyModel>().ReverseMap();
            CreateMap<ApplicantVacancy, ApplicantVacancyModel>().ReverseMap().ForMember(x => x.Vacancy, act => act.Ignore()); ;


            // map Model with DTO
            CreateMap<AppUserModel, AppUserDTO>().ReverseMap();
            CreateMap<VacancyModel, VacancyDTO>().ReverseMap();
            CreateMap<ApplicantVacancyModel, ApplicantVacancyDTO>().ReverseMap();



        }
    }
}

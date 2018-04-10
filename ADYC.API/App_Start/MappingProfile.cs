using ADYC.API.ViewModels;
using ADYC.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADYC.API.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Dto
            CreateMap<Course, CourseDto>();
            CreateMap<CourseType, CourseTypeDto>();

            // Dto to Domain
            CreateMap<CourseForm, Course>()
                .ForMember(c => c.Id, opt => opt.Ignore());
        }
    }
}
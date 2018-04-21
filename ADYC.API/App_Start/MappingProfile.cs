﻿using ADYC.API.ViewModels;
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
            CreateMap<Grade, GradeDto>();
            CreateMap<Major, MajorDto>();
            CreateMap<Period, PeriodDto>();

            // Dto to Domain
            CreateMap<CourseForm, Course>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<CourseTypeForm, CourseType>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<GradeDto, Grade>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<MajorDto, Major>()
                .ForMember(m => m.Id, opt => opt.Ignore());

            CreateMap<PeriodDto, Period>()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
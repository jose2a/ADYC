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
            CreateMap<Grade, GradeDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<Major, MajorDto>();
            CreateMap<Offering, OfferingDto>();
            CreateMap<Period, PeriodDto>();
            CreateMap<PeriodDate, PeriodDateDto>();
            CreateMap<Professor, ProfessorDto>();
            CreateMap<Term, TermDto>();

            // Dto to Domain
            CreateMap<CourseForm, Course>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<CourseTypeForm, CourseType>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<GradeDto, Grade>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<GroupDto, Group>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<MajorDto, Major>()
                .ForMember(m => m.Id, opt => opt.Ignore());

            CreateMap<OfferingDto, Offering>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<PeriodDto, Period>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<PeriodDateDto, PeriodDate>();

            CreateMap<ProfessorDto, Professor>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<TermDto, Term>()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
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
            CreateMap<Enrollment, EnrollmentDto>();
            CreateMap<Evaluation, EvaluationDto>();            
            CreateMap<Grade, GradeDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<Major, MajorDto>();
            CreateMap<Offering, OfferingDto>();
            CreateMap<Period, PeriodDto>();
            CreateMap<PeriodDate, PeriodDateDto>();
            CreateMap<Professor, ProfessorDto>();
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<Student, StudentDto>();
            CreateMap<Term, TermDto>();

            // Dto to Domain
            CreateMap<CourseDto, Course>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<CourseTypeDto, CourseType>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<EnrollmentDto, Enrollment>()
                .ForMember(c => c.Id, opt => opt.Ignore());
                //.ForMember(e => e.Evaluations, opt => opt.MapFrom<EvaluationDto, Evaluation>(e));

            CreateMap<EvaluationDto, Evaluation>();
            CreateMap<IEnumerable<EvaluationDto>, ICollection<Evaluation>>();

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

            CreateMap<ScheduleDto, Schedule>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<StudentDto, Student>()
                .ForMember(p => p.Id, opt => opt.Ignore());

            CreateMap<TermDto, Term>()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
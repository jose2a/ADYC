using ADYC.API.ViewModels;
using ADYC.Model;
using ADYC.Util.RestUtils;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ADYC.API.Controllers
{
    public class ADYCBasedApiController : ApiController
    {
        protected CourseDto GetCourseDto(Course c)
        {
            var courseDto = Mapper.Map<Course, CourseDto>(c);

            courseDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + c.Id;
            courseDto.CourseType = Mapper.Map<CourseType, CourseTypeDto>(c.CourseType);
            courseDto.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + c.CourseTypeId;
            return courseDto;
        }

        protected CourseTypeDto GetCourseTypeDto(CourseType courseType)
        {
            var courseTypeDto = Mapper.Map<CourseType, CourseTypeDto>(courseType);
            courseTypeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + courseType.Id;
            return courseTypeDto;
        }

        protected EnrollmentWithEvaluationsDto GetEnrollmentWithEvaluationsDto(Enrollment enrollment)
        {
            var enrollmentWithEvaluationDto = new EnrollmentWithEvaluationsDto
            {
                Enrollment = GetEnrollmentDto(enrollment),
                Evaluations = enrollment.Evaluations.Select(ev =>
                {
                    var evaluationDto = Mapper.Map<Evaluation, EvaluationDto>(ev);
                    evaluationDto.PeriodGradeLetter = ev.PeriodGradeLetter.ToString();
                    evaluationDto.Period = Mapper.Map<Period, PeriodDto>(ev.Period);
                    evaluationDto.Period.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + ev.PeriodId;

                    return evaluationDto;
                })
            };

            enrollmentWithEvaluationDto.Enrollment.FinalGradeLetter = enrollment.FinalGradeLetter.ToString();

            return enrollmentWithEvaluationDto;
        }

        protected EnrollmentDto GetEnrollmentDto(Enrollment enrollment)
        {
            var enrollmentDto = Mapper.Map<Enrollment, EnrollmentDto>(enrollment);
            enrollmentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Enrollments") + enrollment.Id;

            enrollmentDto.Student = Mapper.Map<Student, StudentDto>(enrollment.Student);
            enrollmentDto.Student.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + enrollment.StudentId;

            enrollmentDto.Student.Grade = Mapper.Map<Grade, GradeDto>(enrollment.Student.Grade);
            enrollmentDto.Student.Grade.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + enrollment.Student.GradeId;

            enrollmentDto.Student.Group = Mapper.Map<Group, GroupDto>(enrollment.Student.Group);
            enrollmentDto.Student.Group.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + enrollment.Student.GroupId;

            enrollmentDto.Student.Major = Mapper.Map<Major, MajorDto>(enrollment.Student.Major);
            enrollmentDto.Student.Major.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + enrollment.Student.MajorId;

            enrollmentDto.Offering = Mapper.Map<Offering, OfferingDto>(enrollment.Offering);
            enrollmentDto.Offering.Url = UrlResoucesUtil.GetBaseUrl(Request, "Enrollments") + enrollment.OfferingId;

            enrollmentDto.Offering.Professor = Mapper.Map<Professor, ProfessorDto>(enrollment.Offering.Professor);
            enrollmentDto.Offering.Professor.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + enrollment.Offering.ProfessorId;

            enrollmentDto.Offering.Course = Mapper.Map<Course, CourseDto>(enrollment.Offering.Course);
            enrollmentDto.Offering.Course.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + enrollment.Offering.CourseId;

            enrollmentDto.Offering.Term = Mapper.Map<Term, TermDto>(enrollment.Offering.Term);
            enrollmentDto.Offering.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + enrollment.Offering.TermId;

            return enrollmentDto;
        }

        protected GradeDto GetGradeDto(Grade g)
        {
            var gradeDto = Mapper.Map<Grade, GradeDto>(g);
            gradeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + g.Id;
            return gradeDto;
        }

        protected GroupDto GetGroupDto(Group g)
        {
            var groupDto = Mapper.Map<Group, GroupDto>(g);
            groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + g.Id;
            return groupDto;
        }

        protected MajorDto GetMajorDto(Major m)
        {
            var majorDto = Mapper.Map<Major, MajorDto>(m);
            majorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + m.Id;
            return majorDto;
        }

        protected OfferingDto GetOfferingDto(Offering offering)
        {
            var offeringDto = Mapper.Map<Offering, OfferingDto>(offering);
            offeringDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offering.Id;

            offeringDto.Professor = Mapper.Map<Professor, ProfessorDto>(offering.Professor);
            offeringDto.Professor.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + offering.ProfessorId;

            offeringDto.Course = Mapper.Map<Course, CourseDto>(offering.Course);
            offeringDto.Course.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + offering.CourseId;

            offeringDto.Course.CourseType = Mapper.Map<CourseType, CourseTypeDto>(offering.Course.CourseType);
            offeringDto.Course.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + offering.Course.CourseTypeId;

            offeringDto.Term = Mapper.Map<Term, TermDto>(offering.Term);
            offeringDto.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + offering.TermId;

            return offeringDto;
        }

        protected PeriodDateListDto GetPeriodDateListDto(int termId, Term term,
            IEnumerable<PeriodDate> periodDates)
        {
            var periodDateListDto = new PeriodDateListDto
            {
                Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + termId + "/PeriodDates",
                PeriodDates = periodDates
                    .Select(pd =>
                    {
                        var periodDateDto = new PeriodDateDto
                        {
                            TermId = pd.TermId,
                            PeriodId = pd.PeriodId,
                            StartDate = pd.StartDate,
                            EndDate = pd.EndDate
                        };

                        periodDateDto.Period = Mapper.Map<Period, PeriodDto>(pd.Period);
                        periodDateDto.Period.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + pd.PeriodId;

                        return periodDateDto;
                    })
            };

            periodDateListDto.Term = Mapper.Map<Term, TermDto>(term);
            periodDateListDto.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + term.Id;

            return periodDateListDto;
        }

        protected PeriodDto GetPeriodDto(Period p)
        {
            var periodDto = Mapper.Map<Period, PeriodDto>(p);
            periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + p.Id;

            return periodDto;
        }

        protected ProfessorDto GetProfessorDto(Professor professor)
        {
            var professorDto = Mapper.Map<Professor, ProfessorDto>(professor);
            professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + professor.Id;

            return professorDto;
        }

        protected ScheduleListDto GetScheduleListDto(int offeringId, Offering offering,
            IEnumerable<Schedule> schedules)
        {
            var scheduleListDto = new ScheduleListDto
            {
                Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId + "/Schedules",
                Schedules = schedules
                    .Select(s =>
                    {
                        return GetScheduleDto(s);
                    })
            };

            scheduleListDto.Offering = Mapper.Map<Offering, OfferingDto>(offering);
            scheduleListDto.Offering.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId;
            scheduleListDto.Offering.Professor.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + offering.ProfessorId;
            scheduleListDto.Offering.Course.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + offering.CourseId;
            scheduleListDto.Offering.Course.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + offering.Course.CourseTypeId;
            scheduleListDto.Offering.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + offering.TermId;

            return scheduleListDto;
        }

        protected ScheduleDto GetScheduleDto(Schedule s)
        {
            var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(s);
            scheduleDto.DayName = s.Day.ToString();

            return scheduleDto;
        }

        protected StudentDto GetStudentDto(Student student)
        {
            var studentDto = Mapper.Map<Student, StudentDto>(student);
            studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + student.Id;

            studentDto.Grade = Mapper.Map<Grade, GradeDto>(student.Grade);
            studentDto.Grade.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + student.GradeId;

            studentDto.Group = Mapper.Map<Group, GroupDto>(student.Group);
            studentDto.Group.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + student.GroupId;

            studentDto.Major = Mapper.Map<Major, MajorDto>(student.Major);
            studentDto.Major.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + student.MajorId;

            return studentDto;
        }

        protected TermDto GetTermDto(Term term)
        {
            var termDto = Mapper.Map<Term, TermDto>(term);
            termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + term.Id;
            return termDto;
        }
    }
}

﻿using ADYC.Data;
using ADYC.IRepository;
using ADYC.IService;
using ADYC.Repository;
using ADYC.Service;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Data.Entity;

namespace ADYC.API.App_Start
{
    public class IocConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // DbContext
            builder.RegisterType<AdycDbContext>().As<DbContext>().InstancePerRequest();

            // Repositories
            builder.RegisterType<CourseRepository>().As<ICourseRepository>().InstancePerRequest();
            builder.RegisterType<CourseTypeRepository>().As<ICourseTypeRepository>().InstancePerRequest();
            builder.RegisterType<EnrollmentRepository>().As<IEnrollmentRepository>().InstancePerRequest();
            builder.RegisterType<EvaluationRepository>().As<IEvaluationRepository>().InstancePerRequest();
            builder.RegisterType<GradeRepository>().As<IGradeRepository>().InstancePerRequest();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>().InstancePerRequest();
            builder.RegisterType<MajorRepository>().As<IMajorRepository>().InstancePerRequest();
            builder.RegisterType<OfferingRepository>().As<IOfferingRepository>().InstancePerRequest();
            builder.RegisterType<PeriodDateRepository>().As<IPeriodDateRepository>().InstancePerRequest();
            builder.RegisterType<PeriodRepository>().As<IPeriodRepository>().InstancePerRequest();
            builder.RegisterType<ProfessorRepository>().As<IProfessorRepository>().InstancePerRequest();
            builder.RegisterType<ScheduleRepository>().As<IScheduleRepository>().InstancePerRequest();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerRequest();
            builder.RegisterType<TermRepository>().As<ITermRepository>().InstancePerRequest();

            // Services
            builder.RegisterType<CourseService>().As<ICourseService>().InstancePerRequest().PropertiesAutowired();
            builder.RegisterType<CourseTypeService>().As<ICourseTypeService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<EnrollmentService>().As<IEnrollmentService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<GradeService>().As<IGradeService>().InstancePerRequest();
            builder.RegisterType<GroupService>().As<IGroupService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<MajorService>().As<IMajorService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<OfferingService>().As<IOfferingService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<PeriodDateService>().As<IPeriodDateService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<PeriodService>().As<IPeriodService>().InstancePerRequest();
            builder.RegisterType<ProfessorService>().As<IProfessorService>().InstancePerRequest();
            builder.RegisterType<ScheduleService>().As<IScheduleService>().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<StudentService>().As<IStudentService>().InstancePerRequest();
            builder.RegisterType<TermService>().As<ITermService>().InstancePerRequest().InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
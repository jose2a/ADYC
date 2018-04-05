using ADYC.Data;
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
            builder.RegisterType<AdycDbContext>().As<DbContext>().InstancePerRequest();

            builder.RegisterType<CourseRepository>().As<ICourseRepository>().InstancePerRequest();
            builder.RegisterType<CourseService>().As<ICourseService>().InstancePerRequest();

            builder.RegisterType<CourseTypeRepository>().As<ICourseTypeRepository>().InstancePerRequest();
            builder.RegisterType<CourseTypeService>().As<ICourseTypeService>().InstancePerRequest();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
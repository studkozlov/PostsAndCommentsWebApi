using Autofac;
using TestApp.BLL.Infrastructure;
using TestApp.BLL.Interfaces;
using TestApp.BLL.Services;

namespace TestApp.API.Utils
{
    public static class AutofacRegistrator
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<PostsAndCommentsManager>().As<ITestAppService>();
            builder.RegisterType<DtoModelsValidator>().As<IDtoModelsValidator>();
        }
    }
}
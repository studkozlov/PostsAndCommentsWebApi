using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.BLL.Interfaces;
using TestApp.BLL.Services;
using TestApp.BLL.Infrastructure;
using Autofac;

namespace TestApp.API.Utils
{
    public static class AutofacRegistrator
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<PostsAndCommentsManager>().As<ITestAppService>();
            builder.RegisterType<DTOModelsValidator>().As<IDTOModelsValidator>();
        }
    }
}
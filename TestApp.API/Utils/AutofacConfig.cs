using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;

namespace TestApp.API.Utils
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            AutofacRegistrator.RegisterDependencies(builder);
            TestApp.BLL.Utils.AutofacRegistrator.RegisterDependencies(builder, "TestAppConnection");
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        
    }
}
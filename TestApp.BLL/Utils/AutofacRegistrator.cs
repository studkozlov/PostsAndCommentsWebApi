using TestApp.DAL.Interfaces;
using TestApp.DAL.DataAccess;
using Autofac;

namespace TestApp.BLL.Utils
{
    public static class AutofacRegistrator
    {
        public static void RegisterDependencies(ContainerBuilder builder, string connectionName)
        {
            builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>().WithParameter("connectionName", connectionName);
        }
    }
}

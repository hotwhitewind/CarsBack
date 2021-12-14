using Autofac;
using Microsoft.EntityFrameworkCore;
using TestCars.Abstraction;
using TestCars.DB;

namespace TestCars
{
    public class AutofacDIModule : Module
    {
        public string ConnectionStringConfig { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                return new SqlDBSettings()
                {
                    ConnectionString = ConnectionStringConfig,
                };
            }).As<ISqlDBSettings>();

            builder.RegisterType<CarsDbContext>().As<DbContext>().InstancePerDependency();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<CarsCRUDService>().InstancePerLifetimeScope();
        }
    }
}

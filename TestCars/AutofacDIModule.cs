using Autofac;
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

            builder.RegisterType<CarsDbContext>().AsSelf();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            //builder.RegisterType<ApplicationDbContext>().As<IUnitOfWork>().InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(MongoDbRepository<>))
            //    .As(typeof(IMongoDbRepository<>))
            //    .SingleInstance();
            //builder.RegisterType<UsersRepository>().As<IUserRepository>().SingleInstance();
            //builder.RegisterType<MocRaveRepository>().As<IRaveRepository>().SingleInstance();
            //builder.RegisterType<AuthHandler>().As<IAuthorizationHandler>();
            //builder.RegisterType<MocEmbededResourceService>().As<IEmbededResourceService>().SingleInstance();
        }
    }
}

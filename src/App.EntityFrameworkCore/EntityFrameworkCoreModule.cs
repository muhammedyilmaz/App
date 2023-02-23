using App.Domain.Repositories;
using App.EntityFrameworkCore;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public class EntityFrameworkCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new AppDbContext(context.Resolve<DbContextOptions<AppDbContext>>()))
                .As<IDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfCoreRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}

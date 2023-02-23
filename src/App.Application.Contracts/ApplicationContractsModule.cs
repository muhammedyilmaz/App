using Autofac;

namespace App
{
    public class ApplicationContractsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<Foo>().As<IFoo>().InstancePerDependency();
        }
    }
}

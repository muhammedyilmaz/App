using Autofac;

namespace App.Web
{
    public class WebCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<Foo>().As<IFoo>().InstancePerDependency();
        }
    }
}

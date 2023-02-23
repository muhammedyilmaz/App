using App.Orders;
using App.Products;
using Autofac;

namespace App
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderAppService>().As<IOrderAppService>().InstancePerDependency();
            builder.RegisterType<ProductAppService>().As<IProductAppService>().InstancePerDependency();
        }
    }
}

using App.EntityFrameworkCore;
using App.Mapper;
using Autofac;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ConfigureAutoMapper();

            ConfigureEntityFramework(services);

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder application)
        {

            if (_env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
                application.UseSwagger();
                application.UseSwaggerUI();
            }

            application.UseRouting();

            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureEntityFramework(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                .AddEntityFrameworkProxies()
                .AddDbContextPool<AppDbContext>(optionsBuilder =>
                {
                    var dbContextOptionsBuilder = optionsBuilder.UseLazyLoadingProxies();
                    var dataConnectionString = _configuration.GetValue<string>("App:DataConnectionString");

                    dbContextOptionsBuilder.UseSqlServer(dataConnectionString);
                });
        }

        private static void ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationMapperConfiguration>();
            });

            AutoMapperConfiguration.Init(config);
        }

        // Dont delete it, It's used by Autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new DomainSharedModule());
            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new ElasticSharedModule());
            builder.RegisterModule(new ElasticModule());
            builder.RegisterModule(new EntityFrameworkCoreModule());
            builder.RegisterModule(new ApplicationContractsModule());
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new WebCoreModule());

        }
    }
}

using Autofac;
using Autofac.Integration.Mvc;
using GamingZone.Data;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(GamingZone.Startup))]
namespace GamingZone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        // Not quite yet. This is only for MVC 5 (in beta)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}

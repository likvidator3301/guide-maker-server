using GuideMaker.Configuration;
using GuideMaker.Middlewares;
using GuideMaker.Repository.PostgreSQL;
using GuideMaker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GuideMaker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(new ExceptionHandlerMiddleware()))
                .AddNewtonsoftJson();

            services.Configure<AuthConfiguration>(Configuration);

            services.AddPostgresRepositories(Configuration.GetSection("Postgres"));

            services.AddSingleton<IGuideDescriptionService, GuideDescriptionService>();
            services.AddSingleton<IGuideService, GuideService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddSingleton<AuthMiddleware>();
            services.AddSingleton<ExceptionHandlerMiddleware>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseMiddleware<AuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

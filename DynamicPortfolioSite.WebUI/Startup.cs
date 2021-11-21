using DynamicPortfolioSite.Repository.Contexts;
using DynamicPortfolioSite.Repository.Repositories.Interfaces;
using DynamicPortfolioSite.Repository.Repositories.Methods;
using DynamicPortfolioSite.Repository.UnitOfWork.Interfaces;
using DynamicPortfolioSite.Repository.UnitOfWork.Methods;
using DynamicPortfolioSite.WebUI.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace DynamicPortfolioSite.WebUI
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
            services
               .AddDbContext<AppDbContext>(optionsAction:
               options => options.UseNpgsql(Configuration.GetConnectionString("AppDbConnection")));

            #region Repositories & UnitOfWork Dependency

            services.AddTransient<IAboutRepository, AboutRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectAndCategoryRepository, ProjectAndCategoryRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IBlogPostRepository, BlogPostRepository>();
            services.AddTransient<IWorkRepository, WorkRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();
            services.AddTransient<IEducationRepository, EducationRepository>();
            services.AddTransient<IAppUserRepository, AppUserRepository>();

            services.AddSingleton<IUnitOfWork, UnitOfWork>();

            #endregion

            services.AddLocalization();

            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResources).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };
                });

            #region Localization Config

            services.Configure<RequestLocalizationOptions>(
                   options =>
                   {
                       var supportedCultures = new List<CultureInfo>
                           {
                            new CultureInfo("tr-TR"),
                            new CultureInfo("en-US"),
                           };

                       options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
                       options.SupportedCultures = supportedCultures;
                       options.SupportedUICultures = supportedCultures;

                       options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                   });

            #endregion

            //AddRazorRuntimeCompilation: Razor sayfalarýndaki deðiþikliði otomatik derler
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

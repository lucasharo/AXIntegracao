using AXIntegracao.Business.Interfaces;
using AXIntegracao.Business.Repositories;
using AXIntegracao.Commom;
using AXIntegracao.Data;
using AXIntegracao.Fornecimento.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace AXIntegracao.Fornecimento
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc(opt=> {
            //    opt.Filters.Add(typeof(ActionFilterAX));
            //    }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient(db => new DapperContext(Configuration.GetSection("ConnectionString").GetValue<string>("DapperConnection")));
            //services.AddScoped<Itipo_comentarioRepository, tipo_comentarioRepository>();
            //services.AddScoped<IHistoricoParecerRepository, HistoricoParecerRepository>();
            //services.AddScoped<Itipo_comentarioRepository, tipo_comentarioRepository>();
            services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
            services.AddScoped(typeof(Resposta<>));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            //Nuget
            //    {
            //    "Microsoft.AspNetCore.Mvc.Versioning"
            //}
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            //Nuget
            //    {
            //    "Swashbuckle.AspNetCore"
            //}

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(Configuration.GetValue<string>("Application:Version"), new Info
                {
                    Version = Configuration.GetValue<string>("Application:Version"),
                    Title = "AXIntegracao",
                    Description = "API de interface",
                    Contact = new Contact { Name = "Audatex", Email = "Audatex@audatex.com.br", Url = "http://www.audatex.com.br" },
                    License = new License { Name = "AX", Url = "https://localhost/api/v1/LICENSE" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            //  app.UseHttpsRedirection();
            app.UseMvc();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile(Configuration.GetValue<string>("LogsDirectory") + Configuration.GetValue<string>("Application:Name") + "/" + "ERROR-{Date}.txt", LogLevel.Error);
            loggerFactory.AddFile(Configuration.GetValue<string>("LogsDirectory") + Configuration.GetValue<string>("Application:Name") + "/" + "WARNING-{Date}.txt", LogLevel.Warning);
            loggerFactory.AddFile(Configuration.GetValue<string>("LogsDirectory") + Configuration.GetValue<string>("Application:Name") + "/" + "INFO-{Date}.txt", LogLevel.Information);

#if DEBUG
            string version = string.Format("/swagger/{0}/swagger.json", Configuration.GetValue<string>("Application:Version"));
#else
            string version = string.Format("/{0}/swagger/{1}/swagger.json", Configuration.GetValue<string>("Application:Name"), Configuration.GetValue<string>("Application:Version"));
#endif

            string versionDescription = string.Format("APi CentralAX versão: {0}", Configuration.GetValue<string>("Application:VersionDescription"));

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint(version, versionDescription);
            });
        }
    }
}

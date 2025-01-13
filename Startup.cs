using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using System.Net.Http;
using MatBlazor;
using Sicem_Blazor.Models;
using Sicem_Blazor.Services;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Filtros;
using Sicem_Blazor.Recaudacion.Data;
using Sicem_Blazor.Eficiencia.Data;
using Sicem_Blazor.ModsUbitoma.Data;
using Sicem_Blazor.Descuentos.Data;
using Sicem_Blazor.PagoLinea.Data;
using Sicem_Blazor.Helpers;
using Sicem_Blazor.Services.Whatsapp;


namespace Sicem_Blazor {
    public class Startup {

        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }


        // This method gets1 called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services){
            services.AddRazorPages();
            services.AddMatBlazor();
            services.AddControllers();
            services.AddDbContext<SicemContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("SICEM"));
            });

            services.AddLogging( config => {
                config.AddConsole();
                config.AddEventSourceLogger();
            });

            services.Configure<BingMapsSettings>(Configuration.GetSection("BingMapsSettings"));
            services.AddSingleton<SessionService>();
            services.AddScoped<SicemService>();
            services.AddScoped<UsersSicemService>();
            services.AddScoped<ConsultaGralService>();
            services.AddScoped<IAtencionService, AtencionService>();
            services.AddScoped<IRecaudacionService, RecaudacionService>();
            services.AddScoped<ITarifasService, TarifasService>();
            services.AddScoped<IUbitomaService, UbitomaService>();
            services.AddScoped<ControlRezagoService>();
            services.AddScoped<MicromedicionService>();
            services.AddScoped<IDescuentosService, DescuentosService>();
            services.AddScoped<IEficienciaService, EficienciaService>();
            services.AddScoped<FacturacionService>();
            services.AddScoped<OrdenesService>(s => new OrdenesService(Configuration, s.GetService<SicemService>()));
            services.AddScoped<PadronService>(s => new PadronService(Configuration, s.GetService<SicemService>()));
            services.AddScoped<ConceptosService>();
            services.AddScoped<LogeadoFilter>();
            services.AddScoped<PagoLineaService>();
            services.AddScoped<AnalisisInformacionService>();
            services.AddScoped<NotificacionesTemplateService>();
            services.AddWhatsappService(Configuration);

            services.AddHttpClient<UbitomaHttpClient>("ubitoma", client => {
                string url = Configuration.GetSection("AppSettings").GetValue<string>("Ubitoma_Api");
                client.BaseAddress = new Uri(url);
            });
            

            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();
            services.AddMatToaster(config =>{
                config.Position = MatToastPosition.TopCenter;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 4000;
            });
            
            services.AddCors(policy => {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            var _ci = new CultureInfo("es-MX");
            app.UseRequestLocalization( new RequestLocalizationOptions(){
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(_ci),
                SupportedCultures = new List<CultureInfo>(){_ci},
                SupportedUICultures = new List<CultureInfo>(){_ci}
            });

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Configuration["SyncfusionSettings:Key"]);

            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                //app.UseHttpsRedirection();
            }
            
            app.UseRequestLocalization( new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture("es-MX"),
                SupportedCultures = new List<CultureInfo>{ new("es-MX")},
                SupportedUICultures = new List<CultureInfo>{ new("es-MX")}
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

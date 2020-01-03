using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CleanMed.Dados.Interface;
using CleanMed.Dados.Repositorio;
using CleanMed.Data;
using CleanMed.Models;
using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;

namespace CleanMed
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
            

            services.AddDbContext<Contexto>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<Usuario,NiveisAcesso>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<Contexto>();

            services.ConfigureApplicationCookie(opcoes => 
            {
                opcoes.AccessDeniedPath = "/Usuarios/AccessDenied";
                opcoes.Cookie.HttpOnly = true;
                opcoes.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                opcoes.LoginPath = "/Usuarios/Login";
                opcoes.SlidingExpiration = true;
            });
           

            services.Configure<IdentityOptions>(opcoes => 
            {
                opcoes.Password.RequireDigit = false;
                opcoes.Password.RequireLowercase = false;
                opcoes.Password.RequireNonAlphanumeric = false;
                opcoes.Password.RequireUppercase = false;
                opcoes.Password.RequiredLength = 6;
                opcoes.Password.RequiredUniqueChars = 1;

            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IPacienteRepositorio, PacienteRepositorio>();
            services.AddScoped<IContatoRepositorio, ContatoRepositorio>();
            services.AddScoped<IConselhoRepositorio, ConselhoRepositorio>();
            services.AddScoped<ITipoPrestadorRepositorio, TipoPrestadorRepositorio>();
            services.AddScoped<IEspecialidadeRepositorio, EspecialidadeRepositorio>();
            services.AddScoped<IPrestadorRepositorio, PrestadorRepositorio>();
            services.AddScoped<IPrestadorEspecialidadeRepositorio, PrestadorEspecialidadeRepositorio>();
            services.AddScoped<IConvenioRepositorio, ConvenioRepositorio>();
            services.AddScoped<ISetorRepositorio, SetorRepositorio>();
            services.AddScoped<IExameRepositorio, ExameRepositorio>();
            services.AddScoped<IGrupoFaturamentoRepositorio, GrupoFaturamentoRepositorio>();
            services.AddScoped<IProcedimentoRepositorio, ProcedimentoRepositorio>();
            services.AddScoped<ITabelaFaturamentoRepositorio, TabelaFaturamentoRepositorio>();
            services.AddScoped<ITabelaFatuProcedimentoRepositorio, TabelaFatuProcedimentoRepositorio>();
            services.AddScoped<IItemAgendamentoRepositorio, ItemAgendamentoRepositorio>();
            services.AddScoped<IItemAgendamentoPrestadorRepositorio, ItemAgendamentoPrestadorRepositorio>();
            services.AddScoped<IRecursoAgendamentoRepositorio, RecursoAgendamentoRepositorio>();
            services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
            services.AddScoped<INivelAcessoRepositorio, NivelAcessoRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IAgendaMedicaRepositorio, AgendaMedicaRepositorio>();
            services.AddScoped<IAgendamentoRepositorio, AgendamentoRepositorio>();
            services.AddScoped<ICartaoConvenioRepositorio, CartaoConvenioRepositorio>();
            // Configuração do JS Report para gerar o relatório em PDF
            services.AddJsReport(new LocalReporting()
                .UseBinary(JsReportBinary.GetBinary())
                .AsUtility()
                .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
          
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            RotativaConfiguration.Setup(env);

            app.UseAuthentication();
            app.UseAuthorization();
       
                app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Usuarios}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
            
        }
    }
}

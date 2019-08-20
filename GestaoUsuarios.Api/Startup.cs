using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Domain.Interfaces;
using GestaoUsuarios.Infra.Data.Context;
using GestaoUsuarios.Infra.Data.Repository;
using GestaoUsuarios.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace GestaoUsuarios.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Utilizar MySql quando o SqlServer não estiver configurado
            if (String.IsNullOrWhiteSpace(Configuration.GetConnectionString("GestaoUsuariosSQL"))){

                //Banco MySql
                services.AddDbContext<GestaoUsuariosContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("GestaoUsuariosMySQL"));
                });

            }
            else
            {

                //Banco SqlServer
                services.AddDbContext<GestaoUsuariosContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("GestaoUsuariosSQL"));
                });

            }

            services.AddTransient<IRepository<Usuario>, UsuarioRepository>();
            services.AddTransient<IService<Usuario>, UsuarioService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("usuarios-authentication-valid")),
                    ClockSkew = TimeSpan.FromMinutes(5),
                    ValidIssuer = "GestaoUsuarios.WebApp",
                    ValidAudience = "Postman",
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}

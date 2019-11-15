using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplicationJWT.Models;
using WebApplicationJWT.Repository;

namespace WebApplicationJWT
{
    public class Startup
    {
        //public class SigningConfigurations
        //{
        //    public SecurityKey Key { get; }
        //    public SigningCredentials SigningCredentials { get; }

        //    public SigningConfigurations()
        //    {
        //        using (var provider = new RSACryptoServiceProvider(2048))
        //        {
        //            Key = new RsaSecurityKey(provider.ExportParameters(true));
        //        }

        //        SigningCredentials = new SigningCredentials(
        //            Key, SecurityAlgorithms.RsaSha256Signature);
        //    }
        //}

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

          

            /*
        services.AddTransient<ApplicationUser>();

        var signingConfigurations = new SigningConfigurations();
        services.AddSingleton(signingConfigurations);

        var tokenConfigurations = new ApplicationSettings();
        new ConfigureFromConfigurationOptions<ApplicationSettings>(
            Configuration.GetSection("ApplicationSettings"))
                .Configure(tokenConfigurations);
        services.AddSingleton(tokenConfigurations);


        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            var paramsValidation = bearerOptions.TokenValidationParameters;
            paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            paramsValidation.ValidAudience = "ExemploAudience";
            paramsValidation.ValidIssuer = "ExemploIssuer";

            // Valida a assinatura de um token recebido
            paramsValidation.ValidateIssuerSigningKey = true;

            // Verifica se um token recebido ainda é válido
            paramsValidation.ValidateLifetime = true;

            // Tempo de tolerância para a expiração de um token (utilizado
            // caso haja problemas de sincronismo de horário entre diferentes
            // computadores envolvidos no processo de comunicação)
            paramsValidation.ClockSkew = TimeSpan.Zero;
        });

        // Ativa o uso do token como forma de autorizar o acesso
        // a recursos deste projeto
        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
        });

        services.AddMvc();
        */

            //Jwt Authentication

            //Inject AppSettings
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<AuthenticationContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<AuthenticationContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            }
            );

            services.AddCors();

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });


            services.AddTransient<IUsuario, Usuario>();
            services.AddTransient<ApplicationUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
            .AllowAnyHeader()
            .AllowAnyMethod()

            );

            app.UseAuthentication();

            app.UseMvc();


            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseMvc();
        }
    }
}

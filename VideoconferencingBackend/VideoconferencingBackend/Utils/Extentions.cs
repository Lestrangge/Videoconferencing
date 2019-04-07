﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces;
using VideoconferencingBackend.Models;
using VideoconferencingBackend.Services;

namespace VideoconferencingBackend.Utils
{
    public static class Extentions
    {
        /// <summary>
        /// Extension method for allowing any request through cors in <see cref="Startup"/>
        /// </summary>
        public static IServiceCollection AddAnyCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", new CorsPolicyBuilder()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .Build()
                );
            });
        }

        /// <summary>
        /// Extension method for adding mvc, correctly working with snake case, in <see cref="Startup"/>
        /// </summary>
        public static void AddSnakeCaseMvc(this IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(jo =>
                {
                    jo.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });
        }

        /// <summary>
        /// Extension method for adding SignalR, correctly working with snake case, in <see cref="Startup"/>
        /// </summary>
        public static void AddSnakeCaseSignalR(this IServiceCollection services, IConfiguration config)
        {
            services.AddSignalR(o => { o.EnableDetailedErrors = true; })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });
            //  .AddRedis(config["Redis"]);
        }

        public static IServiceCollection AddCookiesAuth(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.Headers["Location"] = context.RedirectUri;
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });
            return services.AddAuthorization(options =>
            {
                options.AddPolicy("OperatorPolicy", policy =>
                    policy.RequireRole("admin", "operator", "test"));
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
            });
        }

        public static void AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ICustomAuthenticationService, TokenAuthenticationService>();
            services.AddScoped<ITokenGeneratorService, JwtTokenGenerator>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//debuggy
                        ValidIssuer = config["Issuer"],
                        ValidateAudience = false,//debuggy
                        //ValidAudience = Configuration["Origin"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["TokenSalt"])),
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddAuthorization(options => options.AddPolicy("admin", policy => policy.RequireRole("admin")));
        }

        public static IServiceCollection ConnectToDb(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString),
                ServiceLifetime.Transient);
        }
    }
}

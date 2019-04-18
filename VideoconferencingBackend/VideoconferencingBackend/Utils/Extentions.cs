﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using VideoconferencingBackend.Adapters;
using VideoconferencingBackend.Interfaces.Adapters;
using VideoconferencingBackend.Interfaces.Services.Authentication;
using VideoconferencingBackend.Middleware;
using VideoconferencingBackend.Models;
using VideoconferencingBackend.Services.AuthenticationServices;

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
                        NamingStrategy = new CamelCaseNamingStrategy()
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

        public static void InitFcm(this IServiceCollection services)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });
        }

        public static IApplicationBuilder UseFcmToken(this IApplicationBuilder app)
        {
            return app.UseMiddleware<FcmTokenExtractorMiddleware>();
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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/signalr")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options => options.AddPolicy("admin", policy => policy.RequireRole("admin")));
        }

        public static IServiceCollection ConnectToDb(this IServiceCollection services, string connectionString)
        {
            return services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString),
                ServiceLifetime.Transient);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int? page, int? pageSize)
        {
            if (page == null && pageSize != null)
                return query.Take((int)pageSize);
            if (page == null || pageSize == null)
                return query;
            return query.Skip((int)page * (int)pageSize).Take((int)pageSize);
        }
        public static IServiceCollection AddWebSocket(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IWebSocketAdapter>((serviceProvider) => new WebSocketAdapter(config["Janus"]));
            return services;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }


    }
}

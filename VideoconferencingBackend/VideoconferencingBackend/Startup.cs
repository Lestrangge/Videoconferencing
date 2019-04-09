using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using VideoconferencingBackend.Hubs;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services;
using VideoconferencingBackend.Interfaces.Services.Authentication;
using VideoconferencingBackend.Repositories;
using VideoconferencingBackend.Services;
using VideoconferencingBackend.Services.AuthenticationServices;
using VideoconferencingBackend.Utils;

namespace VideoconferencingBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConnectToDb(Configuration["ConnectionString"]);
            services.AddJwtAuth(Configuration);

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IGroupsRepository, GroupsRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IChatService, ChatService>();

            services.AddSingleton<IHasherService, Sha256Hasher>();
            services.AddMvc();
            services.AddAnyCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Videoconferencing API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSnakeCaseSignalR(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Videoconferencing api V1");
            });
            app.UseCors("SiteCorsPolicy");
            app.UseAuthentication();
            app.UseSignalR(routes => { routes.MapHub<JanusMessagesHub>("/signalr"); });
            app.UseMvc();
        }
    }
}

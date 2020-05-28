using Lamar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;
using VerticalToDo.Infrastructure.Extensions;
using VerticalToDo.Infrastructure.HttpMiddleware;
using VueCliMiddleware;

namespace VerticalToDo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    x.JsonSerializerOptions.MaxDepth = 255;
                });
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            #region setupAuthAndSwagger

            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // Clock skew compensates for server time drift.
                            // We recommend 5 minutes or less:
                            ClockSkew = TimeSpan.FromMinutes(5),
                            // Specify the key used to sign the token:
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VerticalToDoKey")),
                            RequireSignedTokens = true,
                            // Ensure the token hasn't expired:
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            // Ensure the token audience matches our audience value (default true):
                            ValidateAudience = false,
                            // Ensure the token was issued by a trusted authorization server (default true):
                            ValidateIssuer = false,
                        };
                    });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vertical ToDo API", Version = "v1" });
                c.CustomSchemaIds((t) =>
                {
                    return t.FullName.Replace("+", "");
                });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[]{}
                    }
                });
            });

            #endregion

            services.For<IConfiguration>().Use(Configuration);
            services.For<IHttpContextAccessor>().Use<HttpContextAccessor>();
            services.SetupRegistries();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingFilter>();

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.EnableValidator();
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Hiketivity API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "ClientApp";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }

            });
        }
    }
}

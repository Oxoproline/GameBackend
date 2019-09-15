using GameBackend.Constants;
using GameBackend.Infrastracture;
using GameBackend.Services;
using GameBackend.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GameBackend
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
            services.AddControllers();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthenticationConstants.ValidMatchPolicy, builder =>
                {
                    builder
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(AuthenticationConstants.Scheme)
                    .RequireClaim(AuthenticationConstants.UserIdClaim)
                    .RequireClaim(AuthenticationConstants.MatchIdClaim);
                });
            });

            services.AddAuthentication(AuthenticationConstants.Scheme)
                .AddScheme<AuthenticationSchemeOptions, SimpleAuthenticationHandler>(AuthenticationConstants.Scheme, null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Game api", Version = "v1" });

                var scheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token into the field as \"Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                };

                c.AddSecurityDefinition("Bearer", scheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new string[] { }
                    }
                });

                c.DescribeAllEnumsAsStrings();
            });

            services.AddSignalR();

            services
                .AddScoped<IGameAuthenticationService, GameAuthenticationService>()
                .AddSingleton<ITokenService, TokenService>()
                .AddSingleton<IMatchService, MatchService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger()
               .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game api V1");
                    c.RoutePrefix = string.Empty;
                });

            app.UseRouting()
               .UseEndpoints(builder =>
               {
                   builder.MapHub<GameHub>("match/room")
                   .RequireAuthorization();
               });

            app.UseMvcWithDefaultRoute();
        }
    }
}

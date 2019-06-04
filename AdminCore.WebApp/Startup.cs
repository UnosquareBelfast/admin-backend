using System.Security.Claims;
using AdminCore.Common.Authorization;
using AdminCore.Services.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AdminCore.WebApi
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
      services.AddMvc().AddJsonOptions(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling =
          Newtonsoft.Json.ReferenceLoopHandling.Ignore;
      });


      services
        .AddAuthentication(sharedOptions =>
        {
          sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.Audience = Configuration["AzureAd:ClientId"];
          options.Authority = $"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/v2.0/";
        });

      services.AddAuthorization(
//        options =>
//        options.AddPolicy("Admin", policy =>
//        {
//          policy.RequireClaim(ClaimTypes.Role, "Admin");
//        })
      );

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "AdminCore Documentation", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new ApiKeyScheme
        {
          Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
          Name = "Authorization",
          In = "header",
          Type = "apiKey"
        });
      });

      DependencyInjection.RegisterWebDependencyInjection(services);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
      }
      else
      {
        app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
        app.UseHsts();
      }

      app.UseAuthentication();

      app.UseSwagger();
      app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminCore Documentation V1"); });

      app.UseMiddleware(typeof(ErrorHandlingMiddleware));
      app.UseMvc();
    }
  }
}

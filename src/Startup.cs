using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using reading_list_api.Models;
using reading_list_api.Services;

namespace reading_list_api
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
      services.AddCors();
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddEntityFrameworkNpgsql()
        .AddDbContext<ReadingListApiContext>()
        .BuildServiceProvider();

      services.AddAuthentication()
      .AddJwtBearer(cfg =>
      {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;

        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      services.AddScoped<IAuthService, AuthService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}

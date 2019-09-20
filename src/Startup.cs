using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
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
      services.AddMvc()
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
      .AddSessionStateTempDataProvider();

      services.AddSession();

      services.AddEntityFrameworkNpgsql()
        .AddDbContext<ReadingListApiContext>()
        .BuildServiceProvider();

      services.AddAuthentication(options =>
      {
        options.DefaultScheme = "Cookies";
      }).AddCookie("Cookies", options =>
      {
        options.Cookie.Name = "auth_cookie";
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
        options.Events = new CookieAuthenticationEvents
        {
          OnRedirectToLogin = redirectContext =>
          {
            redirectContext.HttpContext.Response.StatusCode = 401;
            return Task.CompletedTask;
          }
        };
      });

      services.AddAuthorization();

      services.AddScoped<IAuthService, AuthService>();
      services.AddHttpContextAccessor();
      services.AddScoped<ISessionService, SessionService>();
      services.AddDistributedMemoryCache();
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
          .WithOrigins("http://localhost:3000")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
      app.UseSession();
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
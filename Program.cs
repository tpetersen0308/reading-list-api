﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using reading_list_api.Models;


namespace reading_list_api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Debug()
      .WriteTo.Console()
      .CreateLogger();

      Helpers.SimpleLogger.Log("Starting Service");

      string json = File.ReadAllText(@"appsettings.json");
      JObject o = JObject.Parse(@json);
      AppSettings.appSettings = JsonConvert.DeserializeObject<AppSettings>(o["AppSettings"].ToString());

      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace CoreWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("[STARTUP] Creating WebApplication builder...");
                var builder = WebApplication.CreateBuilder(args);

                Console.WriteLine("[STARTUP] Adding services...");
                // Add services to the container
                builder.Services
                    .AddControllers()
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver());

                Console.WriteLine("[STARTUP] Building application...");
                var app = builder.Build();

                Console.WriteLine("[STARTUP] Configuring middleware pipeline...");
                // Configure the HTTP request pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseRouting();
                app.MapControllers();

                Console.WriteLine("[STARTUP] Starting Kestrel server...");
                Console.WriteLine("[STARTUP] Listening on http://localhost:5000");
                app.Run("http://localhost:5000");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL] Error: {ex.GetType().Name}");
                Console.WriteLine($"[FATAL] Message: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[FATAL] Inner: {ex.InnerException.Message}");
                }
                Console.WriteLine($"[FATAL] Stack: {ex.StackTrace}");
                Environment.Exit(1);
            }
        }
    }
}

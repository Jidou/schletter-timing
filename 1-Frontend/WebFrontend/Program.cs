using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SchletterTiming.WebFrontend {
    public class Program {
        public static void Main(string[] args) {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:9000/")
                .Build();

            host.Run();
        }
    }
}

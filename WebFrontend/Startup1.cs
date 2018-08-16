using System;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebFrontend.Startup1))]

namespace WebFrontend {
    public class Startup1 {
        public void Configuration(IAppBuilder app) {
#if DEBUG
            app.UseErrorPage();
#endif
            // Remap '/' to '.\defaults\'.
            // Turns on static files and default files.
            //app.UseFileServer(new FileServerOptions() {
            //    RequestPath = PathString.Empty,
            //    FileSystem = new PhysicalFileSystem(@".\Pages"),
            //});

            // Only serve files requested by name.
            app.UseStaticFiles("/Pages");

            // Turns on static files, directory browsing, and default files.
            //app.UseFileServer(new FileServerOptions() {
            //    RequestPath = new PathString("/public"),
            //    EnableDirectoryBrowsing = true,
            //});

            // Browse the root of your application (but do not serve the files).
            // NOTE: Avoid serving static files from the root of your application or bin folder,
            // it allows people to download your application binaries, config files, etc..
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions() {
            //    RequestPath = new PathString("/src"),
            //    FileSystem = new PhysicalFileSystem(@""),
            //});

            // Anything not handled will land at the welcome page.
            //app.UseWelcomePage();

            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "Default",
                "{controller}/{Id}",
                new { controller = "Test", Id = RouteParameter.Optional }
            );

            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            // config.Formatters.JsonFormatter.UseDataContractJsonSerializer = true;

            app.UseWebApi(config);

            app.Run(context => {
                // New code: Throw an exception for this URI path.
                if (context.Request.Path.Equals(new PathString("/fail"))) {
                    throw new Exception("Random exception");
                }

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }
    }
}

using ReaderInterfaces;
using NConfig;
using RunningContext;
using Timy3Reader;
using System;
using System.Configuration;
using System.Linq;
using WebFrontend;

namespace SchletterTiming {
    class Program {

        static void Main(string[] args) {
            NConfigurator.UsingFile(@"Config\Default.config").SetAsSystemDefault();

            InitObjectsFromConfig();

            var baseAddress = ConfigurationManager.AppSettings["BaseAddress"];

            if (string.IsNullOrEmpty(baseAddress)) {
                baseAddress = "http://localhost:9000";
            }

            var readerType = ConfigurationManager.AppSettings["TimyReader"];

            if (string.IsNullOrEmpty(readerType) || readerType == "USB") {
                CurrentContext.Reader = new Timy3UsbReader();
            }

            var startupType = ConfigurationManager.AppSettings["StartupType"];

            if (startupType == "ConsoleOnly") {
                var console = new ConsoleFrontend.Console();
                console.Start();
                return;
            }

            if (startupType == "WebApplication") {
                using (Microsoft.Owin.Hosting.WebApp.Start<Startup1>(baseAddress)) {
                    Console.WriteLine("Press [enter] to quit...");
                    Console.ReadLine();
                }
            }
        }


        private static void InitObjectsFromConfig() {
            InitCategories();
            InitClasses();
        }


        private static void InitCategories() {
            var allCategoriesFromConfigAsString = ConfigurationManager.AppSettings["Categories"];
            var allCategoriesAsList = allCategoriesFromConfigAsString.Split(',').Select(x => x.Trim());
            Category.InitCategories(allCategoriesAsList);
        }


        private static void InitClasses() {
            var allClassesFromConfigAsString = ConfigurationManager.AppSettings["Classes"];
            var allClassesAsList = allClassesFromConfigAsString.Split(',').Select(x => x.Trim());
            Class.InitClasses(allClassesAsList);
        }
    }
}

using NConfig;
using RunningContext;
using System;
using System.Configuration;
using System.Linq;
using Timy3Reader;
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

            if (string.IsNullOrEmpty(readerType)) {
                CurrentContext.Reader = new Timy3MockReader();
            } else if (readerType == "USB") {
                var usbReader = new Timy3UsbReader();
                usbReader.Init();
                CurrentContext.Reader = usbReader;
            } else if (readerType == "RS232") {
                 var rs232Reader = new Timy3RS232Reader();
                rs232Reader.Init();
                CurrentContext.Reader = rs232Reader;
            } else {
                CurrentContext.Reader = new Timy3MockReader();
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

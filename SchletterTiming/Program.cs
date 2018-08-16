using ReaderInterfaces;
using System;
using WebFrontend;

namespace SchletterTiming {
    class Program {

        public static ITimy3Reader reader;

        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            using (Microsoft.Owin.Hosting.WebApp.Start<Startup1>("http://localhost:9000")) {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}

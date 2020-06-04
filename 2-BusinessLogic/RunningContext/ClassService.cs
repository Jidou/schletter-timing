using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningContext {
    public class ClassService {

        private readonly IConfiguration _configuration;

        public static IEnumerable<string> AvailableClasses { get; private set; }


        public ClassService(IConfiguration configuration) {
            _configuration = configuration;
        }


        public void InitClasses(IEnumerable<string> classes) {
            AvailableClasses = classes;
        }


        public void AddClass(string newClass) {
            var tmp = AvailableClasses.ToList();
            tmp.Add(newClass);
            AvailableClasses = tmp;
        }


        public void DeleteClass(string @class) {
            var tmp = AvailableClasses.ToList();
            tmp.Remove(@class);
            AvailableClasses = tmp;
        }


        public void ShowClasses() {
            var allClasses = "";
            foreach (var @class in AvailableClasses) {
                allClasses += $"{@class}\n";
            }
            Console.WriteLine(allClasses);
        }
    }
}

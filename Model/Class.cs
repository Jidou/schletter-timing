using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class Class {
        public static IEnumerable<string> AvailableClasses { get; private set; }


        public static void InitClasses(IEnumerable<string> classes) {
            AvailableClasses = classes;
        }


        public static void AddClass(string newClass) {
            var tmp = AvailableClasses.ToList();
            tmp.Add(newClass);
            AvailableClasses = tmp;
        }


        public static void DeleteClass(string @class) {
            var tmp = AvailableClasses.ToList();
            tmp.Remove(@class);
            AvailableClasses = tmp;
        }


        public static void ShowClasses() {
            var allClasses = "";
            foreach (var @class in AvailableClasses) {
                allClasses += $"{@class}\n";
            }
            Console.WriteLine(allClasses);
        }
    }
}

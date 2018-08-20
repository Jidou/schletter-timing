using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class Category {

        public static IEnumerable<string> AvailableCategories { get; private set; }


        public static void InitCategories(IEnumerable<string> categories) {
            AvailableCategories = categories;
        }


        public static void AddCategory(string newCategory) {
            var tmp = AvailableCategories.ToList();
            tmp.Add(newCategory);
            AvailableCategories = tmp;
        }


        public static void DeleteCategory(string category) {
            var tmp = AvailableCategories.ToList();
            tmp.Remove(category);
            AvailableCategories = tmp;
        }


        public static void ShowCategories() {
            var allCategories = "";
            foreach (var category in AvailableCategories) {
                allCategories += $"{category}\n";
            }
            Console.WriteLine(allCategories);
        }
    }
}

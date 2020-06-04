using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SchletterTiming.RunningContext {
    public class CategoryService {

        private readonly IConfiguration _configuration;

        public static IEnumerable<string> AvailableCategories { get; private set; }


        public CategoryService(IConfiguration configuration) {
            _configuration = configuration;
        }


        public void InitCategories(IEnumerable<string> categories) {
            AvailableCategories = categories;
        }


        public void AddCategory(string newCategory) {
            var tmp = AvailableCategories.ToList();
            tmp.Add(newCategory);
            AvailableCategories = tmp;
        }


        public void DeleteCategory(string category) {
            var tmp = AvailableCategories.ToList();
            tmp.Remove(category);
            AvailableCategories = tmp;
        }


        public void ShowCategories() {
            var allCategories = "";
            foreach (var category in AvailableCategories) {
                allCategories += $"{category}\n";
            }
            Console.WriteLine(allCategories);
        }
    }
}

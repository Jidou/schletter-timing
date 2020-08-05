using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;

namespace SchletterTiming.RunningContext {
    public class CategoryService {

        private const string SaveFileName = "Categories";

        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;


        public CategoryService(IConfiguration configuration, SaveLoad repo) {
            _configuration = configuration;
            _repo = repo;
        }


        public AvailableCategories LoadCategories() {
            return _repo.DeSerializeObject<AvailableCategories>(SaveFileName);
        }


        public void AddCategory(string newCategory) {
            var currentCategories = LoadCategories();
            var categoriesAsList = currentCategories.Categories.ToList();
            categoriesAsList.Add(newCategory);
            currentCategories.Categories = categoriesAsList;
            _repo.SerializeObject(currentCategories, SaveFileName);
        }


        public void DeleteCategory(string category) {
            var currentCategories = LoadCategories();
            var categoriesAsList = currentCategories.Categories.ToList();
            categoriesAsList.Remove(category);
            currentCategories.Categories = categoriesAsList;
            _repo.SerializeObject(currentCategories, SaveFileName);
        }


        public void ShowCategories() {
            var availableCategories = _repo.DeSerializeObject<AvailableCategories>(SaveFileName);
            var allCategories = availableCategories.Categories.Aggregate("", (current, category) => current + $"{category}\n");
            Console.WriteLine(allCategories);
        }
    }
}

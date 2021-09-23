using System;
using System.Collections.Generic;
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


        public IEnumerable<AvailableCategory> LoadCategories() {
            return _repo.DeSerializeObjectFilename<IEnumerable<AvailableCategory>>(SaveFileName) ?? new List<AvailableCategory>();
        }


        public void AddCategory(string newCategory) {
            var currentCategories = LoadCategories();

            if (currentCategories.Any(x => x.CategoryName == newCategory)) {
                return;
            }

            var nextId = 0;

            if (currentCategories.Any()) {
                nextId = currentCategories.Max(x => x.CategoryId) + 1;
            }

            var categoriesAsList = currentCategories.ToList();

            categoriesAsList.Add(new AvailableCategory() {
                CategoryId = nextId,
                CategoryName = newCategory
            });
            
            _repo.SerializeObjectFilename(categoriesAsList, SaveFileName);
        }


        public void UpdateCategory(AvailableCategory category) {
            var currentCategories = LoadCategories();
            var oldCategory = currentCategories.SingleOrDefault(x => x.CategoryId == category.CategoryId);

            if (oldCategory is null) {
                // TODO: Error handling
                return;
            }

            oldCategory.CategoryName = category.CategoryName;
            _repo.SerializeObjectFilename(currentCategories, SaveFileName);
        }


        public void DeleteCategory(string category) {
            var currentCategories = LoadCategories();
            var categoriesAsList = currentCategories.ToList();
            var categoryToDelete = categoriesAsList.Find(x => x.CategoryName == category);

            if (categoryToDelete is null) {
                return;
            }

            categoriesAsList.Remove(categoryToDelete);
            _repo.SerializeObjectFilename(categoriesAsList, SaveFileName);
        }


        public void ShowCategories() {
            var availableCategories = LoadCategories();
            var allCategories = availableCategories.Aggregate("", (current, category) => current + $"{category.CategoryName}\n");
            Console.WriteLine(allCategories);
        }
    }
}

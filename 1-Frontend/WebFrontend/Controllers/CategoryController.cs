using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {


        private readonly CategoryService _categoryService;


        public CategoryController(CategoryService categoryService) {
            _categoryService = categoryService;
        }


        [HttpGet("[action]")]
        public IEnumerable<string> GetAvailableCategories() {
            var availableCategories = _categoryService.LoadCategories();
            return availableCategories.Categories;
        }


        [HttpPost("[action]")]
        public IEnumerable<string> AddCategory([FromBody] string category) {
            _categoryService.AddCategory(category);
            return _categoryService.LoadCategories().Categories;
        }


        [HttpPost("[action]")]
        public IEnumerable<string> DeleteCategory([FromBody] string category) {
            _categoryService.DeleteCategory(category);
            return _categoryService.LoadCategories().Categories;
        }
    }
}

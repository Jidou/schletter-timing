using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {


        private readonly CategoryService _categoryService;


        public CategoryController(CategoryService categoryService) {
            _categoryService = categoryService;
        }


        [HttpGet("[action]")]
        public IEnumerable<Dto.AvailableCategory> GetAvailableCategories() {
            var availableCategories = _categoryService.LoadCategories();
            return ConvertModelToDto(availableCategories);
        }


        [HttpPost("[action]")]
        public IEnumerable<Dto.AvailableCategory> AddCategory([FromBody] Dto.AvailableCategory category) {
            _categoryService.AddCategory(category.CategoryName);
            return ConvertModelToDto(_categoryService.LoadCategories());
        }


        [HttpPost("[action]")]
        public IEnumerable<Dto.AvailableCategory> UpdateCategory([FromBody] Dto.AvailableCategory category) {
            _categoryService.UpdateCategory(ConvertDtoToModel(category));
            return ConvertModelToDto(_categoryService.LoadCategories());
        }


        [HttpPost("[action]")]
        public IEnumerable<Dto.AvailableCategory> DeleteCategory([FromBody] string category) {
            _categoryService.DeleteCategory(category);
            return ConvertModelToDto(_categoryService.LoadCategories());
        }


        private IEnumerable<Dto.AvailableCategory> ConvertModelToDto(IEnumerable<Model.AvailableCategory> availableCategories) {
            return availableCategories.Select(ConvertModelToDto);
        }


        private Dto.AvailableCategory ConvertModelToDto(Model.AvailableCategory availableCategory) {
            return new AvailableCategory {
                CategoryId = availableCategory.CategoryId,
                CategoryName = availableCategory.CategoryName,
            };
        }


        private IEnumerable<Model.AvailableCategory> ConvertDtoToModel(IEnumerable<Dto.AvailableCategory> availableCategories) {
            return availableCategories.Select(ConvertDtoToModel);
        }


        private Model.AvailableCategory ConvertDtoToModel(Dto.AvailableCategory availableCategory) {
            return new Model.AvailableCategory {
                CategoryId = availableCategory.CategoryId,
                CategoryName = availableCategory.CategoryName,
            };
        }
    }
}

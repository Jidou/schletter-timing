using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.Model;
using SchletterTiming.RunningContext;
using AvailableCategory = SchletterTiming.WebFrontend.Dto.AvailableCategory;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase {


        private readonly ClassService _classService;


        public ClassController(ClassService classService) {
            _classService = classService;
        }


        [HttpGet("[action]")]
        public IEnumerable<Dto.AvailableClass> GetAvailableClasses() {
            var availableClasses = _classService.LoadClasses();
            return ConvertModelToDto(availableClasses);
        }


        [HttpPost("[action]")]
        public IEnumerable<Dto.AvailableClass> AddClass([FromBody] AvailableClass @class) {
            _classService.AddClass(@class.ClassName);
            return ConvertModelToDto(_classService.LoadClasses());
        }


        [HttpPost("[action]")]
        public IEnumerable<Dto.AvailableClass> UpdateClass([FromBody] AvailableClass @class) {
            _classService.UpdateClass(@class);
            return ConvertModelToDto(_classService.LoadClasses());
        }


        [HttpPost("[action]")]
        public IEnumerable<Dto.AvailableClass> DeleteClass([FromBody] string @class) {
            _classService.DeleteClass(@class);
            return ConvertModelToDto(_classService.LoadClasses());
        }


        private IEnumerable<Dto.AvailableClass> ConvertModelToDto(IEnumerable<Model.AvailableClass> availableClasses) {
            return availableClasses.Select(ConvertModelToDto);
        }


        private Dto.AvailableClass ConvertModelToDto(Model.AvailableClass availableClass) {
            return new Dto.AvailableClass {
                ClassId = availableClass.ClassId,
                ClassName = availableClass.ClassName
            };
        }
    }
}

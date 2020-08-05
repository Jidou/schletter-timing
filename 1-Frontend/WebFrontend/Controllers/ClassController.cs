using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase {


        private readonly ClassService _classService;


        public ClassController(ClassService classService) {
            _classService = classService;
        }


        [HttpGet("[action]")]
        public IEnumerable<string> GetAvailableClasses() {
            var availableCategories = _classService.LoadClasses();
            return availableCategories.Classes;
        }


        [HttpPost("[action]")]
        public IEnumerable<string> AddClass([FromBody] string @class) {
            _classService.AddClass(@class);
            return _classService.LoadClasses().Classes;
        }


        [HttpPost("[action]")]
        public IEnumerable<string> DeleteClass([FromBody] string @class) {
            _classService.DeleteClass(@class);
            return _classService.LoadClasses().Classes;
        }
    }
}

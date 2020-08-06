using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;
using Race = SchletterTiming.Model.Race;

namespace SchletterTiming.WebFrontend.Controllers {

    [Route("api/[controller]")]
    public class RaceOverviewController : Controller {

        private readonly RaceService _raceService;


        public RaceOverviewController(RaceService raceService) {
            _raceService = raceService;
        }


        [HttpGet()]
        public IEnumerable<RaceOverview> Get() {
            var allRaces = _raceService.GetAllRaces();
            return ConvertModelToDto(allRaces);
        }


        private IEnumerable<RaceOverview> ConvertModelToDto(IEnumerable<Race> allRaces) {
            return allRaces.Select(race => new RaceOverview {
                Name = race.Titel,
                Date = race.Date,
            });
        }
    }
}

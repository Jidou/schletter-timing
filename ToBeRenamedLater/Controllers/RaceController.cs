using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using RunningContext;

namespace ToBeRenamedLater.Controllers {
    [Route("api/[controller]")]
    public class RaceController : Controller {

        private readonly RaceService _raceService;
        private readonly GroupService _groupService;
        private readonly ParticipantService _participantService;
        private readonly TimingValueService _timingValueService;

        public RaceController(RaceService raceService, GroupService groupService, ParticipantService participantService, TimingValueService timingValueService) {
            _raceService = raceService;
            _groupService = groupService;
            _participantService = participantService;
            _timingValueService = timingValueService;
        }


        [HttpGet()]
        public Race Get() {
            var currentRace = CurrentContext.Race;

            if (currentRace == null) {
                currentRace = new Race {
                    Date = DateTime.Today,
                    Judge = string.Empty,
                    Participants = null,
                    Place = string.Empty,
                    RaceType = string.Empty,
                    StartTime = DateTime.Now,
                    TimingTool = TimingTools.AlgeTiming,
                    Titel = string.Empty,
                };

                CurrentContext.Race = currentRace;
            }

            return currentRace;
        }


        [HttpGet("[action]")]
        public Race Load() {
            _raceService.Load("Testing");
            var currentRace = CurrentContext.Race;
            return currentRace;
        }


        [HttpPost()]
        public void Post([FromBody] Race race) {
            CurrentContext.Race = race;

            _raceService.Save("Testing");
        }
    }
}

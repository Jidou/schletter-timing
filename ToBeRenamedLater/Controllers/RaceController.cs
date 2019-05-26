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

        private readonly CurrentContext _currentContext;


        public RaceController(CurrentContext currentContext) {
            _currentContext = currentContext;
        }


        [HttpGet()]
        public Model.Race Get() {
            var currentRace = CurrentContext.Race;

            if (currentRace == null) {
                currentRace = new Model.Race {
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
        public Model.Race Load() {
            RunningContext.Race.Load("Testing");
            var currentRace = CurrentContext.Race;
            return currentRace;
        }


        [HttpPost()]
        public void Post([FromBody] Model.Race race) {
            CurrentContext.Race = race;

            RunningContext.Race.Save("Testing");
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;
using Group = SchletterTiming.Model.Group;
using Race = SchletterTiming.WebFrontend.Dto.Race;

namespace SchletterTiming.WebFrontend.Controllers {
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


        [HttpGet("[action]")]
        public Race LoadRace() {
            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                return ConvertModelToDto(_raceService.CreateEmptyRace());
            } else {
                return ConvertModelToDto(_raceService.LoadCurrentRace());
            }
        }


        [HttpGet("[action]")]
        public bool SetCurrentRace(string racename) {
            _raceService.SetCurrentRace(racename);
            return true;
        }


        [HttpGet("[action]")]
        public Race CreateNewRace() {
            //_raceService.UnsetCurrentRace();
            return ConvertModelToDto(_raceService.CreateEmptyRace());
        }


        [HttpGet("[action]")]
        public IEnumerable<GroupInfoForRace> GetGroupInfoForRace() {
            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                return new List<GroupInfoForRace>();
            }

            var currentRace = _raceService.LoadCurrentRace();
            var raceGroups = currentRace.Groups ?? new List<Group>();
            return ConvertGroupModelsToDto(raceGroups);
        }


        [HttpPost("[action]")]
        public void UpdateRace([FromBody] Race race) {
            var currentRace = ConvertDtoToModel(race);
            _raceService.UpdateRace(currentRace);
        }


        [HttpPost("[action]")]
        public IEnumerable<GroupInfoForRace> AssignStartNumbers() {
            _raceService.AssingStartNumbers();
            var currentRace = _raceService.LoadCurrentRace();
            return ConvertGroupModelsToDto(currentRace.Groups);
        }


        private Model.Race ConvertDtoToModel(Race race) {
            var currentRace = _raceService.LoadCurrentRace();
            var groups = new List<Model.Group>();


            if (currentRace?.Groups != null) {
                groups = currentRace.Groups.ToList();
            }

            return new Model.Race {
                Date = race.Date,
                RaceType = race.RaceType,
                Titel = race.Titel,
                StartTime = race.StartTime,
                Place = race.Place,
                Judge = race.Judge,
                TimingTool = race.TimingTool,
                Groups = groups,
            };
        }


        private Race ConvertModelToDto(Model.Race currentRace) {
            return new Race {
                Titel = currentRace.Titel,
                RaceType = currentRace.RaceType,
                Place = currentRace.Place,
                Judge = currentRace.Judge,
                Date = currentRace.Date,
                TimingTool = currentRace.TimingTool,
                StartTime = currentRace.StartTime,
            };
        }


        private IEnumerable<GroupInfoForRace> ConvertGroupModelsToDto(IEnumerable<Group> groups) {
            var allGroups = groups
                .Select(@group => new GroupInfoForRace {GroupId = @group.GroupId, Groupname = @group.Groupname, StartNumber = @group.StartNumber,})
                .OrderBy(x => x.StartNumber)
                .ToList();

            return allGroups;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.Model;
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
        public Race LoadRace(string racename) {
            if (string.IsNullOrEmpty(racename) && !(CurrentContext.Race is null)) {
                return ConvertModelToDto(CurrentContext.Race);
            }

            _raceService.Load(racename);

            var currentRace = CurrentContext.Race;

            if (currentRace is null) {
                // TODO: check for error and return it.
                return null;
            }

            return ConvertModelToDto(currentRace);
        }


        [HttpGet()]
        public Race Get() {
            if (!(CurrentContext.Race is null)) {
                return ConvertModelToDto(CurrentContext.Race);
            } else {
                // TODO: check for error and return it.
                return null;
            }
        }


        [HttpPut("[action]")]
        public Race CreateNewRace() {
            var newRace = new Model.Race {
                Titel = string.Empty,
                RaceType = string.Empty,
                Place = string.Empty,
                Judge = string.Empty,
                Date = DateTime.Today,
                TimingTool = TimingTools.Unknown,
                StartTime = DateTime.Today,
                Groups = new List<Group>(),
            };

            CurrentContext.Race = newRace;

            return ConvertModelToDto(newRace);
        }


        [HttpGet("[action]")]
        public Race Load() {
            // TODO: find way to remove hard coded file name
            _raceService.Load("Testing");
            var currentRace = CurrentContext.Race;
            return ConvertModelToDto(currentRace);
        }


        [HttpPost()]
        public void Post([FromBody] Race race) {
            var currentRace = ConvertDtoToModel(race);
            CurrentContext.Race = currentRace;
            _raceService.Save("Testing");
        }


        [HttpGet("[action]")]
        public Race AssignStartNumbers() {
            _raceService.AssingStartNumbers();
            var currentRace = CurrentContext.Race;
            _raceService.Save("Testing");
            return ConvertModelToDto(currentRace);
        }
        

        private Model.Race ConvertDtoToModel(Race race) {
            var currentRace = CurrentContext.Race;

            currentRace.Date = race.Date;
            currentRace.RaceType = race.RaceType;
            currentRace.Titel = race.Titel;
            currentRace.StartTime = race.StartTime;
            currentRace.Place = race.Place;
            currentRace.Judge = race.Judge;
            currentRace.TimingTool = race.TimingTool;
            currentRace.Groups = ConvertGroupDtosToModel(race.Groups, currentRace.Groups);

            return currentRace;
        }


        private IEnumerable<Model.Group> ConvertGroupDtosToModel(IEnumerable<GroupInfoForRace> groups, IEnumerable<Model.Group> currentGroups) {
            var availableGroups = CurrentContext.AllAvailableGroups;
            var allGroups = new List<Model.Group>();

            foreach (var group in groups) {
                if (currentGroups.Any(x => x.GroupId == group.GroupId)) {
                    var tmp = currentGroups.Single(x => x.GroupId == group.GroupId);
                    tmp.StartNumber = group.StartNumber;
                    allGroups.Add(tmp);
                } else {
                    var tmp = availableGroups.Single(x => x.GroupId == group.GroupId);
                    tmp.StartNumber = group.StartNumber;
                    allGroups.Add(tmp);
                }
            }

            allGroups = allGroups.OrderBy(x => x.StartNumber).ToList();
            return allGroups;
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
                Groups = ConvertGroupModelsToDto(currentRace.Groups)
            };
        }


        private IEnumerable<GroupInfoForRace> ConvertGroupModelsToDto(IEnumerable<Model.Group> groups) {
            var allGroups = new List<GroupInfoForRace>();

            foreach (var group in groups) {
                allGroups.Add(new GroupInfoForRace {
                    GroupId = group.GroupId,
                    Groupname = group.Groupname,
                    StartNumber = group.StartNumber,
                });
            }

            allGroups = allGroups.OrderBy(x => x.StartNumber).ToList();
            return allGroups;
        }
    }
}

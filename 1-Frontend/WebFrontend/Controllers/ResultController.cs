using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;
using Group = SchletterTiming.Model.Group;
using Race = SchletterTiming.Model.Race;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase {

        private readonly RaceService _raceService;
        private readonly GroupService _groupService;
        private readonly ParticipantService _participantService;
        private readonly TimingValueService _timingValueService;


        public ResultController(RaceService raceService, GroupService groupService, ParticipantService participantService, TimingValueService timingValueService) {
            _raceService = raceService;
            _groupService = groupService;
            _participantService = participantService;
            _timingValueService = timingValueService;
        }


        [HttpGet("[action]")]
        public Dto.Result LoadResult() {

            if (CurrentContext.Race is null) {
                return null;
            }

            return ConvertModelToDto(CurrentContext.Race);
        }

        private Dto.Result ConvertModelToDto(Race race) {
            return new Dto.Result() {
                Titel = race.Titel,
                Date = race.Date.Date.ToLongDateString(),
                Judge = race.Judge,
                Place = race.Place,
                RaceType = race.RaceType,
                StartTime = race.StartTime.TimeOfDay.ToString(),
                TimingTool = race.TimingTool,
                Groups = ConvertGroupModelToGroupResultDto(race.Groups)
            };
        }


        private IEnumerable<GroupResult> ConvertGroupModelToGroupResultDto(IEnumerable<Group> raceGroups) {
            var bestTime = raceGroups.Min(x => x.TimeTaken);

            return raceGroups.Select(raceGroup => ConvertGroupModelToGroupResultDto(raceGroup, bestTime))
                .OrderBy(x => x.FinishTime);
        }


        private GroupResult ConvertGroupModelToGroupResultDto(Group raceGroup, TimeSpan bestTime) {
            return new GroupResult {
                GroupId = raceGroup.GroupId,
                FinishTime = raceGroup.FinishTime,
                Groupname = raceGroup.Groupname,
                Startnumber = raceGroup.StartNumber,
                Participant1Name = $"{raceGroup.Participant1?.Firstname} {raceGroup.Participant1?.Lastname}",
                Participant1Category = $"{raceGroup.Participant1?.Category}",
                Participant2Name = $"{raceGroup.Participant2?.Firstname} {raceGroup.Participant2?.Lastname}",
                Participant2Category = $"{raceGroup.Participant1?.Category}",
                TimeTaken = raceGroup.TimeTaken.ToString(@"c"),
                TimeDiff = (raceGroup.TimeTaken - bestTime).ToString(@"c")
            };
        }
    }
}

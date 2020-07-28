using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.Model;
using SchletterTiming.RunningContext;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TimingController : ControllerBase {

        private readonly RaceService _raceService;
        private readonly GroupService _groupService;
        private readonly ParticipantService _participantService;
        private readonly TimingValueService _timingValueService;


        public TimingController(RaceService raceService, GroupService groupService, ParticipantService participantService, TimingValueService timingValueService) {
            _raceService = raceService;
            _groupService = groupService;
            _participantService = participantService;
            _timingValueService = timingValueService;
        }


        [HttpGet("[action]")]
        public IEnumerable<GroupWithTime> LoadTimingValues() {

            if (CurrentContext.Race is null) {
                return null;
            }

            if (CurrentContext.Race.Groups is null) {
                return null;
            }

            return ConvertModelToDto(CurrentContext.Race.Groups);
        }


        [HttpGet("[action]")]
        public IEnumerable<Dto.TimingValue> GetTimes() {
            var tmpValues = GetTmpValues();

            //var timingValues = CurrentContext.Reader.WaitForBulk();
            //_timingValueService.Save();

            return ConvertTimingValuesModelToDto(tmpValues);
        }

        private List<TimingValue> GetTmpValues() {
            return new List<TimingValue> {
                new TimingValue {
                    MeasurementNumber = 0,
                    StartNumber = 0,
                    Time = "2020-07-28T20:50:00.000",
                },
                new TimingValue {
                    MeasurementNumber = 1,
                    StartNumber = 0,
                    Time = "2020-07-28T20:51:00.000",
                },
                new TimingValue {
                    MeasurementNumber = 2,
                    StartNumber = 0,
                    Time = "2020-07-28T20:53:00.000",
                }
            };
        }


        [HttpPost("[action]")]
        public IEnumerable<GroupWithTime> Assign([FromBody] IEnumerable<Dto.TimingValue> timingValues) {

            CurrentContext.Timing = ConvertTimingValuesDtoToModel(timingValues).ToList();

            _timingValueService.Save();
            _raceService.AddTimingValues();
            _raceService.CalculateFinishTimes();
            _raceService.Save(CurrentContext.Race.Titel);

            return ConvertModelToDto(CurrentContext.Race.Groups);
        }

        private IEnumerable<TimingValue> ConvertTimingValuesDtoToModel(IEnumerable<Dto.TimingValue> timingValues) {
            return timingValues.Select(ConvertTimingValuesDtoToModel).ToList();
        }


        private TimingValue ConvertTimingValuesDtoToModel(Dto.TimingValue timingValue) {
            return new TimingValue {
                Time = timingValue.Time,
                StartNumber = int.Parse(timingValue.StartNumber),
                MeasurementNumber = timingValue.MeasurementNumber,
            };
        }


        private IEnumerable<Dto.TimingValue> ConvertTimingValuesModelToDto(IEnumerable<TimingValue> timingValues) {
            return timingValues.Select(ConvertTimingValuesModelToDto).OrderBy(x => x.MeasurementNumber);
        }


        private Dto.TimingValue ConvertTimingValuesModelToDto(TimingValue timingValue) {
            return new Dto.TimingValue {
                MeasurementNumber = timingValue.MeasurementNumber,
                StartNumber = timingValue.StartNumber.ToString(),
                Time = timingValue.Time,
            };
        }


        private IEnumerable<GroupWithTime> ConvertModelToDto(IEnumerable<Group> groups) {
            return groups.Select(ConvertModelToDto).OrderBy(x => x.Startnumber);
        }


        private GroupWithTime ConvertModelToDto(Group group) {
            return new GroupWithTime {
                Groupname = @group.Groupname,
                Startnumber = @group.StartNumber,
                Participant1Time = @group.Participant1?.FinishTime ?? DateTime.MinValue,
                Participant2Time = @group.Participant2?.FinishTime ?? DateTime.MinValue,
                FinishTime = @group.FinishTime
            };
        }
    }
}

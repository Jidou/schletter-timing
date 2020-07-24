using System;
using System.Collections.Generic;
using System.Linq;
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

            var groups = CurrentContext.Race.Groups;

            return ConvertModelToDto(groups);
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
                    Time = "4564654",
                },
                new TimingValue {
                    MeasurementNumber = 1,
                    StartNumber = 0,
                    Time = "65465465",
                },
                new TimingValue {
                    MeasurementNumber = 2,
                    StartNumber = 0,
                    Time = "654654654",
                }
            };
        }


        private IEnumerable<Dto.TimingValue> ConvertTimingValuesModelToDto(List<TimingValue> timingValues) {
            foreach (var timingValue in timingValues) {
                yield return new Dto.TimingValue {
                    MeasurementNumber = timingValue.MeasurementNumber,
                    StartNumber = timingValue.StartNumber,
                    Time = timingValue.Time,
                };
            }
        }


        private IEnumerable<GroupWithTime> ConvertModelToDto(IEnumerable<Group> groups) {
            foreach (var @group in groups) {
                yield return new GroupWithTime {
                    Groupname = @group.Groupname,
                    Startnumber = @group.StartNumber,
                    Participant1Time = @group.Participant1?.FinishTime ?? DateTime.MinValue,
                    Participant2Time = @group.Participant2?.FinishTime ?? DateTime.MinValue,
                    FinishTime = @group.FinishTime
                };
            }
        }
    }
}

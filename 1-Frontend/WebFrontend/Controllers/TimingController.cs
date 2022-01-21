using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;
using Group = SchletterTiming.Model.Group;
using TimingValue = SchletterTiming.Model.TimingValue;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TimingController : ControllerBase {

        private readonly RaceService _raceService;
        private readonly TimingValueService _timingValueService;


        public TimingController(RaceService raceService, TimingValueService timingValueService) {
            _raceService = raceService;
            _timingValueService = timingValueService;
        }


        [HttpGet("[action]")]
        public IEnumerable<GroupWithTime> LoadTimingValues() {

            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                return null;
            }

            return ConvertModelToDto(_raceService.LoadCurrentRace().Groups);
        }


        [HttpGet("[action]")]
        public IEnumerable<Dto.TimingValue> GetTimes(bool getLiveData) {
            List<TimingValue> timingValues;
            var currentRace = _raceService.LoadCurrentRace();

            if (getLiveData) {
                timingValues = _timingValueService.WaitForBulk();
                _timingValueService.SaveChangesToRaceFolder(currentRace, timingValues);
            } else {
                timingValues = _timingValueService.LoadLatestValuesFromRaceFolder(currentRace.Titel).ToList();

                if (!timingValues.Any()) {
                    timingValues = _timingValueService.WaitForBulk();
                    _timingValueService.SaveChangesToRaceFolder(currentRace, timingValues);
                }
            }


            return ConvertTimingValuesModelToDto(timingValues);
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

            var newTimingValues = ConvertTimingValuesDtoToModel(timingValues).ToList();
            var currentRace = _raceService.LoadCurrentRace();

            _timingValueService.SaveChangesToRaceFolder(currentRace, newTimingValues);

            _raceService.AddTimingValues(currentRace, newTimingValues);
            _raceService.CalculateFinishTimes(currentRace);
            _raceService.UpdateRace(currentRace);

            return ConvertModelToDto(currentRace.Groups);
        }

        private IEnumerable<TimingValue> ConvertTimingValuesDtoToModel(IEnumerable<Dto.TimingValue> timingValues) {
            return timingValues.Select(ConvertTimingValuesDtoToModel).ToList();
        }


        private TimingValue ConvertTimingValuesDtoToModel(Dto.TimingValue timingValue) {
            return new TimingValue {
                Time = timingValue.Time,
                StartNumber = int.Parse(timingValue.StartNumber),
                MeasurementNumber = timingValue.MeasurementNumber,
                InternalId = timingValue.InternalId
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
                InternalId = timingValue.InternalId
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
                FinishTime = @group?.FinishTime ?? DateTime.MinValue,
            };
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Converter;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class RaceGroupController : Controller {

        private readonly RaceService _raceService;
        private readonly ParticipantService _participantService;


        public RaceGroupController(RaceService raceService, ParticipantService participantService) {
            _raceService = raceService;
            _participantService = participantService;
        }


        [HttpGet("[action]")]
        public IEnumerable<Group> GetAllGroupsOfRace() {
            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                return new List<Group>();
            }

            var allGroups = _raceService.LoadCurrentRace().Groups;
            return GroupConverter.ConvertModelToDto(allGroups);
        }


        [HttpPost("[action]")]
        public IEnumerable<Group> UpdateGroups([FromBody] IEnumerable<Group> groups) {
            var currentRace = _raceService.LoadCurrentRace();
            var newGroups = new List<Model.Group>();

            foreach (var @group in groups) {
                Model.Participant participant1;
                Model.Participant participant2;

                var participant1Category = "Läufer";
                string participant2Category;

                if (string.IsNullOrEmpty(@group.Class)) {
                    participant2Category = "Rad";
                } else {
                    participant2Category = @group.Class.Contains("E-Bike") ? "E-Bike" : "Rad";
                }

                if (@group.Participant1Id <= 0) {
                    participant1 = _participantService.LoadOrAddParticipant(@group.Participant1FullName, participant1Category);
                } else {
                    participant1 = _participantService.LoadParticipantById(@group.Participant1Id);

                    CheckParticipantCategory(participant1, participant1Category);
                }


                if (@group.Participant2Id <= 0) {
                    participant2 = _participantService.LoadOrAddParticipant(@group.Participant2FullName, participant2Category);
                } else {
                    participant2 = _participantService.LoadParticipantById(@group.Participant2Id);

                    CheckParticipantCategory(participant2, participant2Category);
                }

                newGroups.Add(GroupConverter.ConvertDtoToModel(@group, participant1, participant2));
            }

            currentRace.Groups = newGroups;

            _raceService.UpdateRace(currentRace);
            return GroupConverter.ConvertModelToDto(newGroups);
        }


        private void CheckParticipantCategory(Model.Participant participant, string category) {
            if (participant.Category != category) {
                participant.Category = category;
                _participantService.UpdateParticipant(participant);
            }
        }
    }
}

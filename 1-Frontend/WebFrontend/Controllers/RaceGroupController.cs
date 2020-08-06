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
        public Group AddGroupToRace([FromBody] Group group) {
            var participant1 = _participantService.LoadParticipantById(group.Participant1Id);
            var participant2 = _participantService.LoadParticipantById(group.Participant2Id);
            var newGroup = GroupConverter.ConvertDtoToModel(group, participant1, participant2);

            _raceService.AddGroup(newGroup);

            return GroupConverter.ConvertModelToDto(newGroup);
        }


        [HttpPost("[action]")]
        public Group UpdateGroupToRace([FromBody] Group group) {
            var currentRace = _raceService.LoadCurrentRace();
            var oldGroup = currentRace.Groups.SingleOrDefault(x => x.GroupId == group.GroupId);

            if (oldGroup is null) {
                return null;
            }

            var groupToUpdate = GroupConverter.ConvertDtoToModel(group, oldGroup.Participant1, oldGroup.Participant2);
            _raceService.UpdateGroups(currentRace, groupToUpdate);
            return GroupConverter.ConvertModelToDto(groupToUpdate);
        }
    }
}

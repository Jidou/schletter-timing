using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog.LayoutRenderers;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Converter;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class GroupController : Controller {

        private readonly GroupService _groupService;
        private readonly ParticipantService _participantService;


        public GroupController(GroupService groupService, ParticipantService participantService) {
            _groupService = groupService;
            _participantService = participantService;
        }


        [HttpGet("[action]")]
        public IEnumerable<Group> GetAllAvailableGroups() {
            var allAvailableGroups = _groupService.LoadAllAvailableGroups();

            return GroupConverter.ConvertModelToDto(allAvailableGroups);
        }


        [HttpPost("[action]")]
        public Group AddGroup([FromBody] Group group) {
            var participant1 = _participantService.LoadParticipantById(group.Participant1Id);
            var participant2 = _participantService.LoadParticipantById(group.Participant2Id);
            var newGroup = GroupConverter.ConvertDtoToModel(group, participant1, participant2);
            newGroup = _groupService.AddGroup(newGroup);
            return GroupConverter.ConvertModelToDto(newGroup);
        }


        [HttpPost("[action]")]
        public Group UpdateGroup([FromBody] Group group) {
            var participant1 = _participantService.LoadParticipantById(group.Participant1Id);
            var participant2 = _participantService.LoadParticipantById(group.Participant2Id);
            var groupToUpdate = GroupConverter.ConvertDtoToModel(group, participant1, participant2);
            groupToUpdate = _groupService.Update(groupToUpdate);
            return GroupConverter.ConvertModelToDto(groupToUpdate);
        }
    }
}

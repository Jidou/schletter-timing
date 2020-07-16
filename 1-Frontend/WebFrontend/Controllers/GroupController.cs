using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class GroupController : Controller {

        private readonly GroupService _groupService;


        public GroupController(GroupService groupService) {
            _groupService = groupService;
        }


        [HttpGet]
        public IEnumerable<Group> Index() {
            var allAvailableGroups = _groupService.LoadAllAvailableGroups();

            return ConvertModelToDto(allAvailableGroups);
        }


        private IEnumerable<Group> ConvertModelToDto(IEnumerable<Model.Group> availableGroups) {
            foreach (var group in availableGroups) {
                yield return new Group {
                    Groupname = group.Groupname,
                    GroupId = group.GroupId,
                    Class = group.Class,
                    Participant1Id = group.Participant1?.ParticipantId ?? 0,
                    Participant1FullName = $"{group.Participant1?.Lastname ?? string.Empty} {group.Participant1?.Firstname ?? string.Empty}",
                    Participant2Id = group.Participant2?.ParticipantId ?? 0,
                    Participant2FullName = $"{group.Participant2?.Lastname ?? string.Empty} {group.Participant2?.Firstname ?? string.Empty}",
                };
            }
        }
    }
}

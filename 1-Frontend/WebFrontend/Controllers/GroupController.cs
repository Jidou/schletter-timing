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


        [HttpPost()]
        public IEnumerable<Group> Post([FromBody] IEnumerable<Group> groups) {
            var groupsToAdd = groups.Where(x => x.ToAdd);
            var groupsToDelete = groups.Where(x => x.ToDelete);
            var groupsToUpdate = groups.Where(x => x.ToUpdate);

            var newGroups = new List<Model.Group>();

            var i = 0;

            if (CurrentContext.AllAvailableGroups.Count > 0) {
                i = CurrentContext.AllAvailableGroups.Max(x => x.GroupId) + 1;
            }

            foreach (var groupToAdd in groupsToAdd) {
                groupToAdd.GroupId = i;
                i++;
            }

            var tmp = groups.Where(x => !x.ToAdd && !x.ToDelete && !x.ToUpdate);
            var groupsToKeep = CurrentContext.AllAvailableGroups.Where(x => tmp.Any(y => y.GroupId == x.GroupId));

            newGroups.AddRange(ConvertDtoToModel(groupsToAdd));
            newGroups.AddRange(ConvertDtoToModel(groupsToUpdate));
            newGroups.AddRange(groupsToKeep);

            CurrentContext.AllAvailableGroups = newGroups.ToList();
            _groupService.Save();

            return ConvertModelToDto(newGroups);
        }


        private IEnumerable<Model.Group> ConvertDtoToModel(IEnumerable<Group> updatedGroups) {
            var currentGroups = CurrentContext.AllAvailableGroups;

            foreach (var group in updatedGroups) {
                var groupToUpdate = currentGroups.SingleOrDefault(x => x.GroupId == group.GroupId);

                if (groupToUpdate == null) {
                    yield return new Model.Group {
                        GroupId = group.GroupId,
                        Class = group.Class,
                        Groupname = group.Groupname,
                        Participant1 = CurrentContext.AllAvailableParticipants.SingleOrDefault(x => x.ParticipantId == group.Participant1Id),
                        Participant2 = CurrentContext.AllAvailableParticipants.SingleOrDefault(x => x.ParticipantId == group.Participant2Id),
                    };
                } else {
                    groupToUpdate.Groupname = group.Groupname;
                    groupToUpdate.Class = group.Class;

                    yield return groupToUpdate;
                }
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog.LayoutRenderers;
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
        public IEnumerable<Group> Get() {
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


        [HttpPost("[action]")]
        public Group AddGroup([FromBody] Group group) {
            var currentMaxGroupId = CurrentContext.AllAvailableGroups.Max(x => x.GroupId);

            var participants = LoadParticipants(group.Participant1Id, group.Participant2Id);
            var newGroup = new Model.Group {
                GroupId = ++currentMaxGroupId,
                Groupname = group.Groupname,
                Class = group.Class,
                Participant1 = participants.Item1,
                Participant2 = participants.Item2,
            };
            
            CurrentContext.AllAvailableGroups.Add(newGroup);

            _groupService.Save();

            return ConvertModelToDto(newGroup);
        }


        [HttpPost("[action]")]
        public Group UpdateGroup([FromBody] Group group) {
            var groupToUpdate = CurrentContext.AllAvailableGroups.SingleOrDefault(x => x.GroupId == group.GroupId);

            if (groupToUpdate is null) {
                // TODO: someone fucked up somewhere, let him know
                return null;
            }

            var indexOfGroup = CurrentContext.AllAvailableGroups.IndexOf(groupToUpdate);

            groupToUpdate.Groupname = group.Groupname;
            groupToUpdate.Class = group.Class;

            CurrentContext.AllAvailableGroups[indexOfGroup] = groupToUpdate;

            _groupService.Save();

            return ConvertModelToDto(groupToUpdate);
        }


        [HttpPost("[action]")]
        public Group UpdateGroupParticipants([FromBody] Group group) {
            var groupToUpdate = CurrentContext.AllAvailableGroups.SingleOrDefault(x => x.GroupId == group.GroupId);

            if (groupToUpdate is null) {
                // TODO: someone fucked up somewhere, let him know
                return null;
            }

            var indexOfGroup = CurrentContext.AllAvailableGroups.IndexOf(groupToUpdate);

            var participants = LoadParticipants(group.Participant1Id, group.Participant2Id);
            groupToUpdate.Participant1 = participants.Item1;
            groupToUpdate.Participant2 = participants.Item2;

            CurrentContext.AllAvailableGroups[indexOfGroup] = groupToUpdate;

            _groupService.Save();

            return ConvertModelToDto(groupToUpdate);
        }


        private Tuple<Model.Participant, Model.Participant> LoadParticipants(int participant1Id, int participant2Id) {
            return new Tuple<Model.Participant, Model.Participant>(
                CurrentContext.AllAvailableParticipants.SingleOrDefault(x => x.ParticipantId == participant1Id),
                CurrentContext.AllAvailableParticipants.SingleOrDefault(x => x.ParticipantId == participant2Id));
            
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
                yield return ConvertModelToDto(group);
            }
        }


        private Group ConvertModelToDto(Model.Group group) {
            return new Group {
                Groupname = group.Groupname,
                GroupId = group.GroupId,
                Class = group.Class,
                Participant1Id = group.Participant1?.ParticipantId ?? 0,
                Participant1FullName = group.Participant1?.Fullname() ?? string.Empty,
                Participant2Id = group.Participant2?.ParticipantId ?? 0,
                Participant2FullName = group.Participant2?.Fullname() ?? string.Empty,
            };
        }
    }
}

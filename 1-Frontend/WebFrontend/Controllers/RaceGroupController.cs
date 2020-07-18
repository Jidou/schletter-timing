using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class RaceGroupController : Controller {

        private readonly GroupService _groupService;
        private readonly RaceService _raceService;


        public RaceGroupController(GroupService groupService, RaceService raceService) {
            _groupService = groupService;
            _raceService = raceService;
        }


        [HttpGet()]
        public IEnumerable<Group> Get() {
            if (CurrentContext.Race is null) {
                return new List<Group>();
            }

            var allGroups = CurrentContext.Race.Groups;

            return ConvertModelToDto(allGroups);
        }


        [HttpGet("[action]")]
        public IEnumerable<GroupInfoForRace> GetGroupInfoForRace(string racename) {
            if (string.IsNullOrEmpty(racename) && !(CurrentContext.Race is null)) {
                return ConvertModelToDtoGroupInfoForRace(CurrentContext.Race.Groups);
            }

            _raceService.Load(racename);

            var raceGroups = CurrentContext.Race.Groups;

            if (raceGroups == null) {
                raceGroups = new List<Model.Group>();
            }

            return ConvertModelToDtoGroupInfoForRace(raceGroups);
        }


        [HttpPost()]
        public IEnumerable<Group> Post([FromBody] IEnumerable<Group> groups) {
            var groupsToAdd = groups.Where(x => x.ToAdd);
            // TODO: delete
            //var groupsToDelete = groups.Where(x => x.ToDelete);

            var newGroups = new List<Model.Group>();

            var tmp = groups.Where(x => !x.ToAdd && !x.ToDelete && !x.ToUpdate);
            var groupsToKeep = CurrentContext.AllAvailableGroups.Where(x => tmp.Any(y => y.GroupId == x.GroupId));

            newGroups.AddRange(ConvertDtoToModel(groupsToAdd));
            newGroups.AddRange(groupsToKeep);

            CurrentContext.Race.Groups = newGroups.ToList();
            _raceService.Save(CurrentContext.Race.Titel);

            return ConvertModelToDto(newGroups);
        }


        [HttpPost()]
        public IEnumerable<Group> UpdateStartNumbers([FromBody] IEnumerable<GroupInfoForRace> groups) {
            var raceGroups = CurrentContext.Race.Groups;

            var enumerable = raceGroups as Model.Group[] ?? raceGroups.ToArray();

            foreach (var @group in groups) {
                foreach (var raceGroup in enumerable) {
                    if (raceGroup.GroupId == @group.GroupId) {
                        raceGroup.StartNumber = @group.StartNumber;
                        break;
                    }
                }
            }

            CurrentContext.Race.Groups = enumerable;
            _raceService.Save(CurrentContext.Race.Titel);

            return ConvertModelToDto(enumerable);
        }


        private IEnumerable<Model.Group> ConvertDtoToModel(IEnumerable<Group> updatedGroups) {
            var currentGroups = CurrentContext.AllAvailableGroups;

            foreach(var group in updatedGroups) {
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
                    Class = group.Class,
                    GroupId = group.GroupId,
                    Participant1Id = group.Participant1?.ParticipantId ?? 0,
                    Participant1FullName = $"{group.Participant1?.Firstname} {group.Participant1?.Lastname}",
                    Participant2Id = group.Participant2?.ParticipantId ?? 0,
                    Participant2FullName = $"{group.Participant2?.Firstname} {group.Participant2?.Lastname}",
                };
            }
        }


        private IEnumerable<GroupIdAndNameOnly> ConvertModelToDtoIdAndNameOnly(IEnumerable<Model.Group> availableGroups) {
            foreach (var group in availableGroups) {
                yield return new GroupIdAndNameOnly {
                    Groupname = group.Groupname,
                    GroupId = group.GroupId,
                };
            }
        }


        private IEnumerable<GroupInfoForRace> ConvertModelToDtoGroupInfoForRace(IEnumerable<Model.Group> availableGroups) {
            foreach (var group in availableGroups) {
                yield return new GroupInfoForRace {
                    Groupname = group.Groupname,
                    GroupId = group.GroupId,
                    StartNumber = group.StartNumber,
                };
            }
        }
    }
}

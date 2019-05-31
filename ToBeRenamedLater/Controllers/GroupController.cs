using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunningContext;
using Model;
using ToBeRenamedLater.Dto;

namespace ToBeRenamedLater.Controllers {
    [Route("api/[controller]")]
    public class GroupController : Controller {

        private readonly GroupService _groupService;


        public GroupController(GroupService groupService) {
            _groupService = groupService;
        }


        [HttpGet()]
        public IEnumerable<Dto.Group> Get() {
            var availableGroups = _groupService.Load();
            var dtos = ConvertModelToDto(availableGroups);
            return dtos;
        }


        [HttpPost()]
        public IEnumerable<Dto.Group> Post([FromBody] IEnumerable<Dto.Group> groups) {
            var groupsToAdd = groups.Where(x => x.ToAdd);
            var groupsToDelete = groups.Where(x => x.ToDelete);
            var groupsToUpdate = groups.Where(x => x.ToUpdate);

            var newGroups = new List<Model.Group>();
            int i = 1;

            foreach (var groupToAdd in groupsToAdd) {
                while(groupsToUpdate.Any(x => x.GroupId == i)) {
                    i++;
                }

                while (groupsToAdd.Any(x => x.GroupId == i)) {
                    i++;
                }

                groupToAdd.GroupId = i;
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


        private IEnumerable<Model.Group> ConvertDtoToModel(IEnumerable<Dto.Group> updatedGroups) {
            var currentGroups = CurrentContext.AllAvailableGroups;

            foreach(var group in updatedGroups) {
                var groupToUpdate = currentGroups.SingleOrDefault(x => x.GroupId == group.GroupId);

                if (groupToUpdate == null) {
                    yield return new Model.Group {
                        GroupId = group.GroupId,
                        Class = group.Class,
                        Groupname = group.Groupname,
                    };
                } else {
                    groupToUpdate.Groupname = group.Groupname;
                    groupToUpdate.Class = group.Class;

                    yield return groupToUpdate;
                }
            }
        }


        private IEnumerable<Dto.Group> ConvertModelToDto(IEnumerable<Model.Group> availableGroups) {
            foreach (var group in availableGroups) {
                yield return new Dto.Group {
                    Groupname = group.Groupname,
                    Class = group.Class,
                    GroupId = group.GroupId,
                };
            }
        }
    }
}

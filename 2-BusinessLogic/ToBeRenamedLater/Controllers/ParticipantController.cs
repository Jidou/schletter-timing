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
    public class ParticipantController : Controller {

        private readonly ParticipantService _participantService;
        private readonly GroupService _groupService;


        public ParticipantController(ParticipantService participantService, GroupService groupService) {
            _participantService = participantService;
            _groupService = groupService;
        }


        [HttpGet()]
        public IEnumerable<Dto.Participant> Get() {
            var availableParticipants = CurrentContext.AllAvailableParticipants;

            if (availableParticipants == null) {
                availableParticipants = _participantService.Load().ToList();
                CurrentContext.AllAvailableParticipants = availableParticipants;
            }

            var availableGroups = CurrentContext.AllAvailableGroups;

            if (availableGroups == null) {
                availableGroups = _groupService.Load().ToList();
                CurrentContext.AllAvailableGroups = availableGroups;
            }

            var dtos = ConvertModelToDto(availableParticipants, availableGroups);
            return dtos;
        }


        [HttpPost()]
        public IEnumerable<Dto.Participant> Post([FromBody] IEnumerable<Dto.Participant> participants) {
            var participantsToAdd = participants.Where(x => x.ToAdd);
            var participantsToDelete = participants.Where(x => x.ToDelete);
            var participantsToUpdate = participants.Where(x => x.ToUpdate);

            var newParticipants = UpdateParticipants(participants, participantsToAdd, participantsToUpdate);
            UpdateGroups(participantsToAdd, participantsToUpdate, participantsToDelete);

            _participantService.Save();
            _groupService.Save();

            return ConvertModelToDto(newParticipants, CurrentContext.AllAvailableGroups);
        }


        private void UpdateGroups(IEnumerable<Dto.Participant> participantsToAdd, IEnumerable<Dto.Participant> participantsToUpdate, IEnumerable<Dto.Participant> participantsToDelete) {
            var allGroups = CurrentContext.AllAvailableGroups;

            CheckAndRemoveFromGroups(participantsToDelete, allGroups);
            CheckAndUpdateGroups(participantsToUpdate, allGroups);
            CheckAndAddToGroups(participantsToAdd, allGroups);

            CurrentContext.AllAvailableGroups = allGroups;
        }


        private void CheckAndAddToGroups(IEnumerable<Dto.Participant> participantsToAdd, List<Model.Group> allGroups) {
            foreach (var participantToAdd in participantsToAdd) {
                var newGroup = allGroups.SingleOrDefault(x => x.GroupId == participantToAdd.GroupId);

                if (newGroup == null) {
                    continue;
                }

                if (newGroup.Participant1 == null) {
                    newGroup.Participant1 = ConvertDtoToModelSingle(CurrentContext.AllAvailableParticipants, participantToAdd);
                } else if (newGroup.Participant2 == null) {
                    newGroup.Participant2 = ConvertDtoToModelSingle(CurrentContext.AllAvailableParticipants, participantToAdd);
                } else {
                    // TODO: throw exception, to show someone fucked up
                }
            }
        }


        private static void CheckAndUpdateGroups(IEnumerable<Dto.Participant> participantsToUpdate, List<Model.Group> allGroups) {
            foreach (var participantToUpdate in participantsToUpdate) {
                CheckAndRemoveFromGroupsSingle(allGroups, participantToUpdate);
            }

            foreach (var participantToUpdate in participantsToUpdate) {
                var newGroup = allGroups.SingleOrDefault(x => x.GroupId == participantToUpdate.GroupId);

                if (newGroup == null) {
                    continue;
                }

                if (newGroup.Participant1 == null) {
                    newGroup.Participant1 = ConvertDtoToModelSingle(CurrentContext.AllAvailableParticipants, participantToUpdate);
                } else if (newGroup.Participant2 == null) {
                    newGroup.Participant2 = ConvertDtoToModelSingle(CurrentContext.AllAvailableParticipants, participantToUpdate);
                } else {
                    // TODO: throw exception, to show someone fucked up
                }
            }
        }


        private static void CheckAndRemoveFromGroups(IEnumerable<Dto.Participant> participantsToDelete, List<Model.Group> allGroups) {
            foreach (var participantToDelete in participantsToDelete) {
                CheckAndRemoveFromGroupsSingle(allGroups, participantToDelete);
            }
        }


        private static void CheckAndRemoveFromGroupsSingle(List<Model.Group> allGroups, Dto.Participant participantToDelete) {
            if (allGroups.Any(x => x.Participant1?.ParticipantId == participantToDelete.ParticipantId)) {
                var toRemove = allGroups.Where(x => x.Participant1?.ParticipantId == participantToDelete.ParticipantId);
                foreach (var tr in toRemove) {
                    tr.Participant1 = null;
                }
            }

            if (allGroups.Any(x => x.Participant2?.ParticipantId == participantToDelete.ParticipantId)) {
                var toRemove = allGroups.Where(x => x.Participant2?.ParticipantId == participantToDelete.ParticipantId);
                foreach (var tr in toRemove) {
                    tr.Participant2 = null;
                }
            }
        }

        private List<Model.Participant> UpdateParticipants(IEnumerable<Dto.Participant> participants, IEnumerable<Dto.Participant> participantsToAdd, IEnumerable<Dto.Participant> participantsToUpdate) {
            var newParticipants = new List<Model.Participant>();
            var i = CurrentContext.AllAvailableParticipants.Max(x => x.ParticipantId) + 1;

            foreach (var participantToAdd in participantsToAdd) {
                participantToAdd.ParticipantId = i;
                i++;
            }

            var tmp = participants.Where(x => !x.ToAdd && !x.ToDelete && !x.ToUpdate);
            var participantsToKeep = CurrentContext.AllAvailableParticipants.Where(x => tmp.Any(y => y.ParticipantId == x.ParticipantId));


            newParticipants.AddRange(ConvertDtoToModel(participantsToAdd));
            newParticipants.AddRange(ConvertDtoToModel(participantsToUpdate));
            newParticipants.AddRange(participantsToKeep);

            CurrentContext.AllAvailableParticipants = newParticipants.ToList();
            return newParticipants;
        }


        private IEnumerable<Model.Participant> ConvertDtoToModel(IEnumerable<Dto.Participant> newParticipants) {
            var currentParticipants = CurrentContext.AllAvailableParticipants;

            foreach(var participant in newParticipants) {
                yield return ConvertDtoToModelSingle(currentParticipants, participant);
            }
        }


        private static Model.Participant ConvertDtoToModelSingle(List<Model.Participant> currentParticipants, Dto.Participant participant) {
            var participantToUpdate = currentParticipants.SingleOrDefault(x => x.ParticipantId == participant.ParticipantId);

            if (participantToUpdate == null) {
                return new Model.Participant {
                    Category = participant.Category,
                    ParticipantId = participant.ParticipantId,
                    Firstname = participant.Firstname,
                    Lastname = participant.Lastname,
                    YearOfBirth = participant.YearOfBirth,
                    GroupId = participant.GroupId,
                };
            } else {
                participantToUpdate.Category = participant.Category;
                participantToUpdate.Firstname = participant.Firstname;
                participantToUpdate.Lastname = participant.Lastname;
                participantToUpdate.YearOfBirth = participant.YearOfBirth;
                participantToUpdate.GroupId = participant.GroupId;

                return participantToUpdate;
            }
        }

        private IEnumerable<Dto.Participant> ConvertModelToDto(IEnumerable<Model.Participant> availableParticipants, IEnumerable<Model.Group> availableGroups) {
            foreach (var participant in availableParticipants) {
                yield return new Dto.Participant {
                    Category = participant.Category,
                    ParticipantId = participant.ParticipantId,
                    Firstname = participant.Firstname,
                    Lastname = participant.Lastname,
                    YearOfBirth = participant.YearOfBirth,
                    GroupId = participant.GroupId,
                    GroupName = participant.GroupId > 0 
                        ? availableGroups.FirstOrDefault(x => x.GroupId == participant.GroupId).Groupname 
                        : string.Empty,
                };
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class RaceParticipantController : Controller {

        private readonly ParticipantService _participantService;
        private readonly GroupService _groupService;


        public RaceParticipantController(ParticipantService participantService, GroupService groupService) {
            _participantService = participantService;
            _groupService = groupService;
        }


        [HttpGet()]
        public IEnumerable<Participant> Get() {
            if (CurrentContext.Race is null) {
                return new List<Participant>();
            }

            var allParticipants = CurrentContext.Race.Groups
                .Where(y => y.Participant1 != null)
                .Select(x => x.Participant1)
                .Union(CurrentContext.Race.Groups
                    .Where(y => y.Participant2 != null)
                    .Select(x => x.Participant2));
            
            return ConvertModelToDto(allParticipants);
        }


        [HttpPost()]
        public IEnumerable<Participant> Post([FromBody] IEnumerable<Participant> participants) {
            var participantsToAdd = participants.Where(x => x.ToAdd).ToList();
            var participantsToDelete = participants.Where(x => x.ToDelete).ToList();
            var participantsToUpdate = participants.Where(x => x.ToUpdate).ToList();

            var newParticipants = UpdateParticipants(participants, participantsToAdd, participantsToUpdate);
            UpdateGroups(participantsToAdd, participantsToUpdate, participantsToDelete);

            _participantService.Save();
            _groupService.Save();

            return ConvertModelToDto(newParticipants);
        }


        private void UpdateGroups(IEnumerable<Participant> participantsToAdd, IEnumerable<Participant> participantsToUpdate, IEnumerable<Participant> participantsToDelete) {
            var allGroups = CurrentContext.AllAvailableGroups;

            CheckAndRemoveFromGroups(participantsToDelete, allGroups);
            CheckAndUpdateGroups(participantsToUpdate, allGroups);
            CheckAndAddToGroups(participantsToAdd, allGroups);

            CurrentContext.AllAvailableGroups = allGroups;
        }


        private void CheckAndAddToGroups(IEnumerable<Participant> participantsToAdd, List<Model.Group> allGroups) {
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


        private static void CheckAndUpdateGroups(IEnumerable<Participant> participantsToUpdate, List<Model.Group> allGroups) {
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


        private static void CheckAndRemoveFromGroups(IEnumerable<Participant> participantsToDelete, List<Model.Group> allGroups) {
            foreach (var participantToDelete in participantsToDelete) {
                CheckAndRemoveFromGroupsSingle(allGroups, participantToDelete);
            }
        }


        private static void CheckAndRemoveFromGroupsSingle(List<Model.Group> allGroups, Participant participantToDelete) {
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

        private List<Model.Participant> UpdateParticipants(IEnumerable<Participant> participants, IEnumerable<Participant> participantsToAdd, IEnumerable<Participant> participantsToUpdate) {
            var newParticipants = new List<Model.Participant>();

            var i = 0;

            if (CurrentContext.AllAvailableParticipants.Count > 0) {
                i = CurrentContext.AllAvailableParticipants.Max(x => x.ParticipantId) + 1;
            }

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


        private IEnumerable<Model.Participant> ConvertDtoToModel(IEnumerable<Participant> newParticipants) {
            var currentParticipants = CurrentContext.AllAvailableParticipants;

            foreach(var participant in newParticipants) {
                yield return ConvertDtoToModelSingle(currentParticipants, participant);
            }
        }


        private static Model.Participant ConvertDtoToModelSingle(List<Model.Participant> currentParticipants, Participant participant) {
            var participantToUpdate = currentParticipants.SingleOrDefault(x => x.ParticipantId == participant.ParticipantId);

            if (participantToUpdate == null) {
                return new Model.Participant {
                    Category = participant.Category,
                    ParticipantId = participant.ParticipantId,
                    Firstname = participant.Firstname,
                    Lastname = participant.Lastname,
                    YearOfBirth = participant.YearOfBirth,
                };
            } else {
                participantToUpdate.Category = participant.Category;
                participantToUpdate.Firstname = participant.Firstname;
                participantToUpdate.Lastname = participant.Lastname;
                participantToUpdate.YearOfBirth = participant.YearOfBirth;

                return participantToUpdate;
            }
        }

        private IEnumerable<Participant> ConvertModelToDto(IEnumerable<Model.Participant> availableParticipants) {
            foreach (var participant in availableParticipants) {
                yield return new Participant {
                    Category = participant.Category,
                    ParticipantId = participant.ParticipantId,
                    Firstname = participant.Firstname,
                    Lastname = participant.Lastname,
                    YearOfBirth = participant.YearOfBirth,
                };
            }
        }
    }
}

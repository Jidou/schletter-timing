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


        public ParticipantController(ParticipantService participantService) {
            _participantService = participantService;
        }


        [HttpGet()]
        public IEnumerable<Dto.Participant> Get() {
            var availableParticipants = _participantService.Load();
            var dtos = ConvertModelToDto(availableParticipants);
            return dtos;
        }


        [HttpPost()]
        public IEnumerable<Dto.Participant> Post([FromBody] IEnumerable<Dto.Participant> participants) {
            var participantsToAdd = participants.Where(x => x.ToAdd);
            var participantsToDelete = participants.Where(x => x.ToDelete);
            var participantsToUpdate = participants.Where(x => x.ToUpdate);

            var newParticipants = new List<Model.Participant>();
            int i = 1;

            foreach (var participantToAdd in participantsToAdd) {
                while(participantsToUpdate.Any(x => x.ParticipantId == i)) {
                    i++;
                }

                while (participantsToAdd.Any(x => x.ParticipantId == i)) {
                    i++;
                }

                participantToAdd.ParticipantId = i;
            }

            var tmp = participants.Where(x => !x.ToAdd && !x.ToDelete && !x.ToUpdate);
            var participantsToKeep = CurrentContext.AllAvailableParticipants.Where(x => tmp.Any(y => y.ParticipantId == x.ParticipantId));


            newParticipants.AddRange(ConvertDtoToModel(participantsToAdd));
            newParticipants.AddRange(ConvertDtoToModel(participantsToUpdate));
            newParticipants.AddRange(participantsToKeep);

            CurrentContext.AllAvailableParticipants = newParticipants.ToList();
            _participantService.Save();

            return ConvertModelToDto(newParticipants);
        }


        private IEnumerable<Model.Participant> ConvertDtoToModel(IEnumerable<Dto.Participant> newParticipants) {
            var currentParticipants = CurrentContext.AllAvailableParticipants;

            foreach(var participant in newParticipants) {
                var groupToUpdate = currentParticipants.SingleOrDefault(x => x.ParticipantId == participant.ParticipantId);

                if (groupToUpdate == null) {
                    yield return new Model.Participant {
                        Category = participant.Category,
                        ParticipantId = participant.ParticipantId,
                        Firstname = participant.Firstname,
                        Lastname = participant.Lastname,
                        YearOfBirth = participant.YearOfBirth
                    };
                } else {
                    groupToUpdate.Category = participant.Category;
                    groupToUpdate.Firstname = participant.Firstname;
                    groupToUpdate.Lastname = participant.Lastname;
                    groupToUpdate.YearOfBirth = participant.YearOfBirth;

                    yield return groupToUpdate;
                }
            }
        }


        private IEnumerable<Dto.Participant> ConvertModelToDto(IEnumerable<Model.Participant> availableGroups) {
            foreach (var participant in availableGroups) {
                yield return new Dto.Participant {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class ParticipantController : Controller {

        private readonly ParticipantService _participantService;
        private readonly GroupService _groupService;


        public ParticipantController(ParticipantService participantService, GroupService groupService) {
            _participantService = participantService;
            _groupService = groupService;
        }


        [HttpGet]
        public IEnumerable<Participant> Get() {

            var allAvailableParticipants = _participantService.LoadAllAvailableParticipants();

            return ConvertModelToParticipantDto(allAvailableParticipants);
        }


        [HttpGet("[action]")]
        public IEnumerable<ParticipantSuggestions> GetAllParticipantsWithoutGroup() {

            var allAvailableParticipants = _participantService.LoadAllAvailableParticipants();
            var allGropus = _groupService.LoadAllAvailableGroups();

            var allParticipantsIds = new List<int>();

            foreach (var @group in allGropus) {
                if (@group.Participant1 != null) {
                    allParticipantsIds.Add(@group.Participant1.ParticipantId);
                }

                if (@group.Participant2 != null) {
                    allParticipantsIds.Add(@group.Participant2.ParticipantId);
                }
            }

            var participantsWithoutGroup =
                allAvailableParticipants.Where(x => allParticipantsIds.All(y => y != x.ParticipantId));

            return ConvertModelToOtherParticipantSuggestionsDto(participantsWithoutGroup);
        }


        [HttpPost()]
        public IEnumerable<Participant> Post([FromBody] IEnumerable<Participant> participants) {
            var participantsToAdd = participants.Where(x => x.ToAdd).ToList();
            // TODO: Implement delete
            var participantsToDelete = participants.Where(x => x.ToDelete).ToList();
            var participantsToUpdate = participants.Where(x => x.ToUpdate).ToList();

            var newParticipants = UpdateParticipants(participants, participantsToAdd, participantsToUpdate);

            _participantService.Save();

            return ConvertModelToParticipantDto(newParticipants);
        }


        [HttpPost("[action]")]
        public Participant AddParticipant([FromBody] Participant participant) {
            var currentMaxId = CurrentContext.AllAvailableParticipants.Max(x => x.ParticipantId);

            var newParticipant = new Model.Participant {
                ParticipantId = ++currentMaxId,
                Firstname = participant.Firstname,
                Lastname = participant.Lastname,
                Category = participant.Category,
                YearOfBirth = participant.YearOfBirth,
            };

            CurrentContext.AllAvailableParticipants.Add(newParticipant);

            _participantService.Save();

            return ConvertModelToParticipantDto(newParticipant);
        }


        [HttpPost("[action]")]
        public Participant UpdateParticipant([FromBody] Participant participant) {
            var oldParticipant =
                CurrentContext.AllAvailableParticipants.SingleOrDefault(x =>
                    x.ParticipantId == participant.ParticipantId);

            if (oldParticipant is null) {
                // TODO: someone fucked up somewhere, let him know
                return null;
            }

            oldParticipant.Firstname = participant.Firstname;
            oldParticipant.Lastname = participant.Lastname;
            oldParticipant.Category = participant.Category;
            oldParticipant.YearOfBirth = participant.YearOfBirth;
            
            _participantService.Save();

            return ConvertModelToParticipantDto(oldParticipant);
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


            newParticipants.AddRange(ConvertParticipantDtoToModel(participantsToAdd));
            newParticipants.AddRange(ConvertParticipantDtoToModel(participantsToUpdate));
            newParticipants.AddRange(participantsToKeep);

            CurrentContext.AllAvailableParticipants = newParticipants.ToList();
            return newParticipants;
        }


        private IEnumerable<Model.Participant> ConvertParticipantDtoToModel(IEnumerable<Participant> participantsToUpdate) {
            foreach (var participant in participantsToUpdate) {
                yield return new Model.Participant(participant.Firstname, participant.Lastname, participant.YearOfBirth, participant.Category);
            }
        }


        private IEnumerable<ParticipantSuggestions> ConvertModelToOtherParticipantSuggestionsDto(IEnumerable<Model.Participant> participantsWithoutGroup) {
            foreach (var participant in participantsWithoutGroup) {
                yield return new ParticipantSuggestions {
                    ParticipantId = participant.ParticipantId,
                    Fullname = participant.Fullname(),
                };
            }
        }


        private IEnumerable<Participant> ConvertModelToParticipantDto(IEnumerable<Model.Participant> allAvailableParticipants) {
            foreach (var participant in allAvailableParticipants) {
                yield return ConvertModelToParticipantDto(participant);
            }
        }


        private Participant ConvertModelToParticipantDto(Model.Participant participant) {
            return new Participant {
                Category = participant.Category,
                ParticipantId = participant.ParticipantId,
                Firstname = participant.Firstname,
                Lastname = participant.Lastname,
                YearOfBirth = participant.YearOfBirth,
            };
        }
    }
}

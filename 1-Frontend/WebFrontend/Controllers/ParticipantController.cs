using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Converter;
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


        [HttpGet("[action]")]
        public IEnumerable<Participant> GetAllAvailableParticipants() {
            var allAvailableParticipants = _participantService.LoadAllAvailableParticipants();
            return ParticipantConverter.ConvertModelToDto(allAvailableParticipants);
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


        [HttpPost("[action]")]
        public Participant AddParticipant([FromBody] Participant participant) {
            var participantToAdd = ParticipantConverter.ConvertDtoToModel(participant);
            var newParticipant = _participantService.AddParticipant(participantToAdd);
            return ParticipantConverter.ConvertModelToDto(newParticipant);
        }


        [HttpPost("[action]")]
        public Participant UpdateParticipant([FromBody] Participant participant) {
            var participantToUpdate = ParticipantConverter.ConvertDtoToModel(participant);
            _participantService.UpdateParticipant(participantToUpdate);
            return ParticipantConverter.ConvertModelToDto(participantToUpdate);
        }


        private IEnumerable<ParticipantSuggestions> ConvertModelToOtherParticipantSuggestionsDto(IEnumerable<Model.Participant> participantsWithoutGroup) {
            return participantsWithoutGroup.Select(participant => new ParticipantSuggestions {
                ParticipantId = participant.ParticipantId,
                Fullname = participant.Fullname(),
            });
        }
    }
}

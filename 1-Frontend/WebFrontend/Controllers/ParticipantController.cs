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


        public ParticipantController(ParticipantService participantService) {
            _participantService = participantService;
        }


        [HttpGet("[action]")]
        public IEnumerable<Participant> GetAllAvailableParticipants() {
            var allAvailableParticipants = _participantService.LoadAllAvailableParticipants();
            return ParticipantConverter.ConvertModelToDto(allAvailableParticipants);
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

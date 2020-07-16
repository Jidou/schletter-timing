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


        public ParticipantController(ParticipantService participantService) {
            _participantService = participantService;
        }


        [HttpGet]
        public IEnumerable<Participant> Get() {

            var allAvailableParticipants = _participantService.LoadAllAvailableParticipants();

            return ConvertModelToDto(allAvailableParticipants);
        }


        private IEnumerable<Participant> ConvertModelToDto(IEnumerable<Model.Participant> allAvailableParticipants) {
            foreach (var participant in allAvailableParticipants) {
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

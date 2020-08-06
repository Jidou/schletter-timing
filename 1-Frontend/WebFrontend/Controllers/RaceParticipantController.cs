using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class RaceParticipantController : Controller {

        private readonly ParticipantService _participantService;
        private readonly RaceService _raceService;
        private readonly GroupService _groupService;


        public RaceParticipantController(ParticipantService participantService, GroupService groupService, RaceService raceService) {
            _participantService = participantService;
            _groupService = groupService;
            _raceService = raceService;
        }


        [HttpGet()]
        public IEnumerable<Participant> GetAllParticipantsOfRace() {
            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                return new List<Participant>();
            }

            var currentRace = _raceService.LoadCurrentRace();

            var allParticipants = currentRace.Groups
                .Where(y => y.Participant1 != null)
                .Select(x => x.Participant1)
                .Union(currentRace.Groups
                    .Where(y => y.Participant2 != null)
                    .Select(x => x.Participant2));
            
            return ConvertModelToDto(allParticipants);
        }


        [HttpPost()]
        public IEnumerable<Participant> Post([FromBody] IEnumerable<Participant> participants) {
            //var participantsToAdd = participants.Where(x => x.ToAdd).ToList();
            //var participantsToDelete = participants.Where(x => x.ToDelete).ToList();
            //var participantsToUpdate = participants.Where(x => x.ToUpdate).ToList();

            //var newParticipants = UpdateParticipants(participants, participantsToAdd, participantsToUpdate);
            //UpdateGroups(participantsToAdd, participantsToUpdate, participantsToDelete);

            //_participantService.SaveChangesToRaceFolder();
            //_groupService.SaveChangesToRaceFolder();

            //return ConvertModelToDto(newParticipants);
            return null;
        }


        private IEnumerable<Model.Participant> ConvertDtoToModel(IEnumerable<Participant> newParticipants) {
            var currentParticipants = _participantService.LoadAllAvailableParticipants().ToList();

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

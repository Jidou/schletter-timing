using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Converter {
    internal static class ParticipantConverter {

        public static IEnumerable<Model.Participant> ConvertDtoToModel(IEnumerable<Participant> participantsToUpdate) {
            return participantsToUpdate.Select(ConvertDtoToModel);
        }


        public static Model.Participant ConvertDtoToModel(Participant participant) {
            return new Model.Participant {
                ParticipantId = participant.ParticipantId,
                Firstname = participant.Firstname,
                Lastname = participant.Lastname,
                YearOfBirth = participant.YearOfBirth,
                Category = participant.Category,
            };
        }


        public static IEnumerable<Participant> ConvertModelToDto(IEnumerable<Model.Participant> allAvailableParticipants) {
            return allAvailableParticipants.Select(ConvertModelToDto);
        }


        public static Participant ConvertModelToDto(Model.Participant participant) {
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

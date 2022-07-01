using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchletterTiming.WebFrontend.Dto;

namespace SchletterTiming.WebFrontend.Converter {
    internal static class GroupConverter {

        public static Model.Group ConvertDtoToModel(Group group, Model.Participant participant1, Model.Participant participant2) {

            return new Model.Group {
                GroupId = group.GroupId,
                Groupname = group.Groupname,
                Class = group.Class,
                StartNumber = group.StartNumber,
                FinishTime = null,
                TimeTaken = null,
                Participant1 = participant1,
                Participant2 = participant2,
            };
        }


        public static IEnumerable<Group> ConvertModelToDto(IEnumerable<Model.Group> availableGroups) {
            return availableGroups.Select(ConvertModelToDto).OrderBy(x => x.StartNumber);
        }


        public static Group ConvertModelToDto(Model.Group group) {
            return new Group {
                Groupname = group.Groupname,
                GroupId = group.GroupId,
                Class = group.Class,
                StartNumber = group.StartNumber,
                Participant1Id = group.Participant1?.ParticipantId ?? 0,
                Participant1FullName = group.Participant1?.Fullname() ?? string.Empty,
                Participant2Id = group.Participant2?.ParticipantId ?? 0,
                Participant2FullName = group.Participant2?.Fullname() ?? string.Empty,
            };
        }
    }
}

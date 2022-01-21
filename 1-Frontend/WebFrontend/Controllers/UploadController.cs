using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SchletterTiming.RunningContext;
using SchletterTiming.WebFrontend.Dto;
using Group = SchletterTiming.Model.Group;
using Race = SchletterTiming.WebFrontend.Dto.Race;

namespace SchletterTiming.WebFrontend.Controllers {
    [Route("api/[controller]")]
    public class UploadController : Controller {

        private readonly RaceService _raceService;
        private readonly GroupService _groupService;
        private readonly ParticipantService _participantService;


        public UploadController(RaceService raceService, GroupService groupService, ParticipantService participantService) {
            _raceService = raceService;
            _groupService = groupService;
            _participantService = participantService;
        }


        [HttpPost("[action]")]
        public Race Upload([FromBody] IEnumerable<Dto.Upload> uploads) {
            var runners = uploads.Select(x => x.Participant1);
            var bikers = uploads.Select(x => x.Participant2);

            _participantService.CheckUpload(runners, "Läufer");
            _participantService.CheckUpload(bikers, "Radfahrer");

            var uploadsModel = uploads.Select(ConvertDtoToModel);

            _raceService.CheckUpload(uploadsModel);

            return ConvertModelToDto(_raceService.LoadCurrentRace());
        }


        private Model.Upload ConvertDtoToModel(Upload upload) {
            var participant1 = _participantService.LoadParticipantByName(upload.Participant1);
            var participant2 = _participantService.LoadParticipantByName(upload.Participant2);

            return new Model.Upload
            {
                Groupname = upload.GroupName,
                Participant1 = participant1,
                Participant2 = participant2,
            };
        }


        private Race ConvertModelToDto(Model.Race currentRace) {
            return new Race {
                Titel = currentRace.Titel,
                RaceType = currentRace.RaceType,
                Place = currentRace.Place,
                Judge = currentRace.Judge,
                Date = currentRace.Date,
                TimingTool = currentRace.TimingTool,
                StartTime = currentRace.StartTime,
                Groups = ConvertGroupModelsToDto(currentRace.Groups),
            };
        }


        private IEnumerable<GroupInfoForRace> ConvertGroupModelsToDto(IEnumerable<Group> groups) {
            var allGroups = groups
                .Select(@group => new GroupInfoForRace { GroupId = @group.GroupId, Groupname = @group.Groupname, StartNumber = @group.StartNumber, })
                .OrderBy(x => x.StartNumber)
                .ToList();

            return allGroups;
        }
    }
}

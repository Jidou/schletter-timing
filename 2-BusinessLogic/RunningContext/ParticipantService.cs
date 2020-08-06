using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;

namespace SchletterTiming.RunningContext {
    public class ParticipantService {
        private const string SaveFileName = "Participants";

        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;


        public ParticipantService(IConfiguration configuration, SaveLoad repo) {
            _configuration = configuration;
            _repo = repo;
        }


        public IEnumerable<Participant> LoadAllAvailableParticipants() {
            var participants = _repo.DeSerializeObjectFilename<IEnumerable<Participant>>(SaveFileName);

            if (participants != null) {
                return participants;
            }

            participants = new List<Participant>();
            return participants;
        }


        public Participant LoadParticipantById(int participantId) {
            return LoadAllAvailableParticipants().SingleOrDefault(x => x.ParticipantId == participantId);
        }


        public Participant LoadParticipantByName(string partIdent) {
            return LoadAllAvailableParticipants().SingleOrDefault(x => $"{x.Firstname}_{x.Lastname}" == partIdent);
        }


        public Participant AddParticipant(Participant participant) {
            var allParticipants = LoadAllAvailableParticipants().ToList();
            var nextId = allParticipants.Max(x => x.ParticipantId) + 1;
            participant.ParticipantId = nextId;
            allParticipants.Add(participant);
            _repo.SerializeObjectFilename(allParticipants, SaveFileName);
            return participant;
        }


        public void UpdateParticipant(Participant participantToUpdate) {
            var allParticipants = LoadAllAvailableParticipants().ToList();
            var oldParticipant = allParticipants.Find(x => x.ParticipantId == participantToUpdate.ParticipantId);
            allParticipants[allParticipants.IndexOf(oldParticipant)] = participantToUpdate;
            _repo.SerializeObjectFilename(allParticipants, SaveFileName);
        }
    }
}

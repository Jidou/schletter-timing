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


        public void Save() {
            var allParticipants = CurrentContext.AllAvailableParticipants;
            _repo.SerializeObject(allParticipants, SaveFileName);
        }


        public IEnumerable<Participant> Load() {
            var participants = (IEnumerable<Participant>)CurrentContext.AllAvailableParticipants;
            if (participants != null) {
                return participants;
            }

            participants = _repo.DeSerializeObject<IEnumerable<Participant>>(SaveFileName);

            if (participants != null) {
                CurrentContext.AllAvailableParticipants = participants.ToList();
                return participants;
            }

            participants = new List<Participant>();
            CurrentContext.AllAvailableParticipants = participants.ToList();
            return participants;
        }
    }
}

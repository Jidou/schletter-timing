using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace RunningContext {
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


        public IEnumerable<Model.Participant> Load() {
            var participants = (IEnumerable<Model.Participant>)CurrentContext.AllAvailableParticipants;
            if (participants != null) {
                return participants;
            }

            participants = _repo.DeSerializeObject<IEnumerable<Model.Participant>>(SaveFileName);

            if (participants != null) {
                CurrentContext.AllAvailableParticipants = participants.ToList();
                return participants;
            }

            participants = new List<Model.Participant>();
            CurrentContext.AllAvailableParticipants = participants.ToList();
            return participants;
        }
    }
}

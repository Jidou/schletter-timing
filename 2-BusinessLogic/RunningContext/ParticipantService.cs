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


        //public Participant LoadParticipantByName(string partIdent) {
        //    return LoadAllAvailableParticipants().SingleOrDefault(x => $"{x.Firstname}_{x.Lastname}" == partIdent);
        //}


        public Participant LoadParticipantByName(string name) {
            return LoadAllAvailableParticipants().SingleOrDefault(x => CompareName(x.Firstname, x.Lastname, name));
        }


        public Participant AddParticipant(Participant participant) {
            var allParticipants = LoadAllAvailableParticipants().ToList();

            var nextId = 0;

            if (allParticipants.Any()) {
                nextId = allParticipants.Max(x => x.ParticipantId) + 1;
            }

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


        public void CheckUpload(IEnumerable<string> allParticipants, string category) {
            var allAvailableParticipants = LoadAllAvailableParticipants();

            foreach (var participant in allParticipants) {
                var nameSplit = participant.Split(' ');
                var found = false;

                foreach (var availableParticipant in allAvailableParticipants) {
                    if (CompareName(availableParticipant.Firstname, availableParticipant.Lastname, participant)) {

                        if (availableParticipant.Category != category) {
                            availableParticipant.Category = category;
                            UpdateParticipant(availableParticipant);
                        } else {
                            found = true;
                        }

                        break;
                    }
                }

                if (!found) {
                    AddParticipant(new Participant {
                        Firstname = nameSplit[0],
                        Lastname = nameSplit[1],
                        Category = category,
                    });
                }
            }
        }


        private bool CompareName(string firstname, string lastname, string fullname) {
            var nameSplit = fullname.Split(' ');

            return (firstname == nameSplit[0] && lastname == nameSplit[1]) ||
                   (firstname == nameSplit[1] && lastname == nameSplit[0]);
        }
    }
}

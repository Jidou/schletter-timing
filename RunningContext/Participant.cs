using System;
using System.Linq;
using System.Collections.Generic;

namespace RunningContext {
    public class Participant {
        public static void Save(string filename) {
            Save(filename, false);
        }


        public static void SaveFromRace(string filename) {
            Save(filename, true);
        }


        private static void Save(string filename, bool saveFromRace) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"participants_tmp_{CurrentContext.SaveCounter++}";
            }

            var allParticipants = new List<Model.Participant>();

            if (saveFromRace) {
                foreach (var group in CurrentContext.Race.Participants) {
                    allParticipants.Add(group.Participant1);
                    allParticipants.Add(group.Participant2);
                }
            } else {
                allParticipants = CurrentContext.AllAvailableParticipants;
            }

            SaveLoad.SerializeObject(allParticipants, filename);
        }


        public static void Load(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                return;
                //filename = $"tmp_{CurrentContext.SaveCounter++}";
            }

            var loadedParticipants = SaveLoad.DeSerializeObject<IEnumerable<Model.Participant>>(filename).ToList();
            CurrentContext.AllAvailableParticipants = loadedParticipants;
        }
    }
}

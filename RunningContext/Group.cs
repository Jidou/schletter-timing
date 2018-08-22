using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace RunningContext {
    public class Group {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void AddParticipants(string[] input) {
            int.TryParse(input[0], out int groupnumber);
            var group = CurrentContext.AllAvailableGroups.SingleOrDefault(x => x.Groupname == input[0] || x.Groupnumber == groupnumber);

            if (group == null) {
                logger.Info($"Unable to find group {input[0]}");
                return;
            }

            var part1Ident = input[1];
            var part1 = CurrentContext.AllAvailableParticipants.SingleOrDefault(x => $"{x.Firstname}_{x.Lastname}" == part1Ident);

            if (part1 == null) {
                logger.Info($"Unable to find participant {input[1]}");
                return;
            }

            var part2Ident = input[2];
            var part2 = CurrentContext.AllAvailableParticipants.SingleOrDefault(x => $"{x.Firstname}_{x.Lastname}" == part2Ident);

            if (part1 == null) {
                logger.Info($"Unable to find participant {input[2]}");
                return;
            }

            group.Participant1 = part1;
            group.Participant2 = part2;

            logger.Info($"Group successfully updated");
        }


        public static void Save(string filename) {
            Save(filename, false);
        }


        public static void SaveFromRace(string filename) {
            Save(filename, true);
        }


        public static void Load(string filename) {
            Load(filename, false);
        }


        public static void LoadFromRace(string filename) {
            Load(filename, true);
        }


        private static void Save(string filename, bool saveFromRace) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"groups_tmp_{CurrentContext.SaveCounter++}";
            }

            var allParticipants = CurrentContext.AllAvailableGroups;
            SaveLoad.SerializeObject(allParticipants, filename);
        }


        private static void Load(string filename, bool loadIntoRace) {
            if (string.IsNullOrEmpty(filename)) {
                return;
                //filename = $"tmp_{CurrentContext.SaveCounter++}";
            }

            var loadedGroups = SaveLoad.DeSerializeObject<IEnumerable<Model.Group>>(filename).ToList();

            if (loadIntoRace) {
                CurrentContext.Race.Participants = loadedGroups;
            } else {
                CurrentContext.AllAvailableGroups = loadedGroups;
            }
        }
    }
}

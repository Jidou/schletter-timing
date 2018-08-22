using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningContext {
    public class Race {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();


        public static void SetStartTime(string startTime) {
            var time = DateTime.Parse(startTime);
            CurrentContext.Race.StartTime = time;
        }


        public static void AddGroup(string[] input) {
            if (CurrentContext.Race == null) {
                logger.Info($"No race created yet");
                return;
            }

            var currentRaceParticipants = CurrentContext.Race.Participants.ToList();

            foreach (var groupIdentifier in input) {
                int.TryParse(groupIdentifier, out int groupnumber);
                var group = CurrentContext.AllAvailableGroups.SingleOrDefault(x => x.Groupname == groupIdentifier || x.Groupnumber == groupnumber);

                if (group == null) {
                    logger.Info($"Unable to find group {groupIdentifier}");
                    return;
                }

                if (currentRaceParticipants.Contains(group)) {
                    logger.Info($"Group {groupIdentifier} is already part of this race");
                    continue;
                }

                currentRaceParticipants.Add(group);
                logger.Info($"Group {groupIdentifier} successfully added to race");
            }

            CurrentContext.Race.Participants = currentRaceParticipants;
        }


        public static void AddTimingValues() {
            var allGroups = CurrentContext.Race.Participants;

            foreach (var group in allGroups) {
                var finishTimeOfGroup = CurrentContext.Timing.SingleOrDefault(x => x.Groupnumber == group.Groupnumber);

                if (finishTimeOfGroup == null) {
                    logger.Info($"Could not find finish time for group {group.Groupname}");
                    continue;
                }

                group.FinishTime = DateTime.Parse(finishTimeOfGroup.Time);
            }
        }


        public static void Save(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"race_tmp_{CurrentContext.SaveCounter++}";
            }

            SaveLoad.SerializeObject<Model.Race>(CurrentContext.Race, filename);
        }


        public static void Load(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                return;
                //filename = $"tmp_{CurrentContext.SaveCounter++}";
            }

            var loadedScenario = SaveLoad.DeSerializeObject<Model.Race>(filename);
            CurrentContext.Race = loadedScenario;
        }
    }
}

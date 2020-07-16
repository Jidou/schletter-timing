using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NLog;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;

namespace SchletterTiming.RunningContext {
    public class RaceService {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;


        public RaceService(IConfiguration configuration, SaveLoad repo) {
            _configuration = configuration;
            _repo = repo;
        }


        public void SetStartTime(string startTime) {
            var time = DateTime.Parse(startTime);
            CurrentContext.Race.StartTime = time;
        }


        public IEnumerable<Race> GetAllRaces() {
            var path = $"{Environment.CurrentDirectory}\\Data\\Races";

            var allFiles = Directory.GetFiles(path);

            var allRaces = new List<Race>();

            foreach (var file in allFiles) {
                allRaces.Add(_repo.DeSerializeObject<Race>(file));
            }

            return allRaces;
        }


        public void AddGroup(string[] input) {
            if (CurrentContext.Race == null) {
                logger.Info($"No race created yet");
                return;
            }

            var currentRaceParticipants = CurrentContext.Race.Groups.ToList();

            foreach (var groupIdentifier in input) {
                int.TryParse(groupIdentifier, out int startNumber);
                var group = CurrentContext.AllAvailableGroups.SingleOrDefault(x => x.Groupname == groupIdentifier || x.StartNumber == startNumber);

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

            CurrentContext.Race.Groups = currentRaceParticipants;
        }


        public void AddTimingValues() {
            var race = CurrentContext.Race;
            var allGroups = race.Groups;

            foreach (var group in allGroups) {
                var finishTimeOfGroup = CurrentContext.Timing.SingleOrDefault(x => x.StartNumber == group.StartNumber);

                if (finishTimeOfGroup == null) {
                    logger.Info($"Could not find finish time for group {group.Groupname}");
                    continue;
                }

                group.FinishTime = DateTime.Parse(finishTimeOfGroup.Time);
                group.TimeTaken = group.FinishTime - race.StartTime;
            }
        }


        public void AssingStartNumbers() {
            var race = CurrentContext.Race;
            Shuffle(race, true);
        }


        private void Shuffle(Race race, bool shuffleAll = false) {
            var nextStartNumber = 1;

            if (shuffleAll) {
                foreach (var group in race.Groups) {
                    group.StartNumber = 0;
                }
            }

            var rand = new Random(DateTime.Now.Millisecond);
            var groups = race.Groups.ToArray();
            var numberOfGroups = race.Groups.Count();

            for (var i = 0; i < numberOfGroups; i++) {
                int nextGroup;

                do {
                    nextGroup = rand.Next(0, numberOfGroups);

                    if (groups[nextGroup].StartNumber == 0) {
                        groups[nextGroup].StartNumber = nextStartNumber;
                        nextStartNumber++;
                        break;
                    }

                } while (true);
            }

            race.Groups = groups.ToList();
            CurrentContext.Race = race;
        }


        public void CalculateFinishTimes() {
            var race = CurrentContext.Race;
            var allGroups = race.Groups;

            foreach (var group in allGroups) {
                group.TimeTaken = group.FinishTime - race.StartTime;
            }
        }


        public void Save(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"race_tmp_{CurrentContext.SaveCounter++}";
            }

            filename = $"Races/{filename}";

            _repo.SerializeObject<Race>(CurrentContext.Race, filename);
        }


        public void Load(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                return;
            }

            filename = $"Races/{filename}";

            var loadedScenario = _repo.DeSerializeObject<Race>(filename);
            CurrentContext.Race = loadedScenario;
        }


        public void Reset() {
            CurrentContext.Race = new Race {
                Date = DateTime.Today,
                Judge = string.Empty,
                Groups = new List<Group>(),
                Place = string.Empty,
                RaceType = string.Empty,
                StartTime = DateTime.Now,
                TimingTool = TimingTools.AlgeTiming,
                Titel = string.Empty,
            };
        }
    }
}

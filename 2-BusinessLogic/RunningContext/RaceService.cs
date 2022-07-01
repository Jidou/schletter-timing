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
        private const string RacesBaseFolder = "Races";
        private const string DefaultRaceTitel = "Titel";

        private static readonly string RacesBasePath = $"{Environment.CurrentDirectory}\\Data\\Races";
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;
        private readonly TimingValueService _timingValueService;


        public RaceService(IConfiguration configuration, SaveLoad repo, TimingValueService timingValueService) {
            _configuration = configuration;
            _repo = repo;
            _timingValueService = timingValueService;
        }


        public Race AddRace(Race race) {
            _repo.SerializeObjectFilename(race, $"{RacesBaseFolder}/{race.Titel}");
            SetCurrentRace(race.Titel);
            return race;
        }


        public IEnumerable<Race> GetAllRaces() {
            var allFiles = Directory.GetFiles(RacesBasePath);

            return allFiles.Select(file => _repo.DeSerializeObjectFilename<Race>(file)).ToList();
        }


        public void SetCurrentRace(string raceName) {
            CurrentContext.CurrentRaceTitle = raceName;
        }


        public void UnsetCurrentRace() {
            CurrentContext.CurrentRaceTitle = string.Empty;
        }


        public Race LoadCurrentRace() =>
            !string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle) 
                ? LoadRace(CurrentContext.CurrentRaceTitle) 
                : null;


        public Race LoadRace(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                return null;
            }

            filename = $"{RacesBaseFolder}/{filename}";

            return _repo.DeSerializeObjectFilename<Race>(filename);
        }


        public void SetStartTime(string startTime) {
            var race = LoadRace(CurrentContext.CurrentRaceTitle);
            var time = DateTime.Parse(startTime);
            race.StartTime = time;
            Update(race);
        }


        public void AddGroup(Group group) {
            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                Logger.Info($"No race created yet");
                return;
            }

            var currentRace = LoadCurrentRace();
            var currentGroups = currentRace.Groups;
            var currentGroupsAsList = currentGroups.ToList();
            currentGroupsAsList.Add(group);

            currentRace.Groups = currentGroupsAsList;
            Update(currentRace);
        }


        public void AddTimingValues(Race currentRace, IEnumerable<TimingValue> timingValues) {
            var allGroups = currentRace.Groups;

            foreach (var group in allGroups) {
                var finishTimeOfGroup = timingValues.FirstOrDefault(x => x.StartNumber == group.StartNumber);

                if (finishTimeOfGroup == null) {
                    Logger.Info($"Could not find finish time for group {group.Groupname}");
                    continue;
                }

                group.FinishTime = DateTime.Parse(finishTimeOfGroup.Time);
                group.TimeTaken = group.FinishTime - currentRace.StartTime;
            }
        }


        public void AssingStartNumbers() {
            var race = LoadCurrentRace();
            Shuffle(race, true);
            Update(race);
        }


        public void CalculateFinishTimes(Race currentRace) {
            var allGroups = currentRace.Groups;

            foreach (var group in allGroups) {
                group.TimeTaken = group.FinishTime - currentRace.StartTime;
            }
        }


        public void CheckUpload(IEnumerable<Model.Upload> uploads) {
            var race = LoadCurrentRace();
            var groups = race.Groups.ToList();
            var groupId = 0;

            foreach (var upload in uploads) {
                if (groups.Any(x => x.Groupname == upload.Groupname)) {
                    var group = race.Groups.Single(x => x.Groupname == upload.Groupname);

                    if (group.Participant1 == upload.Participant1 && group.Participant2 == upload.Participant2) {
                        continue;
                    } else {
                        group.Participant1 = upload.Participant1;
                        group.Participant2 = upload.Participant2;
                    }
                } else {
                    groups.Add(new Group {
                        Class = upload.Class,
                        GroupId = groupId,
                        Groupname = upload.Groupname,
                        Participant1 = upload.Participant1,
                        Participant2 = upload.Participant2,
                    });
                }

                groupId++;
            }

            race.Groups = groups;

            Update(race);
        }


        private void Update(Race race) {
            var filename = $"{RacesBaseFolder}/{race.Titel}";
            _repo.SerializeObjectFilename<Race>(race, filename);
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
        }


        public void UpdateRace(Race currentRace) {
            if (string.IsNullOrEmpty(CurrentContext.CurrentRaceTitle)) {
                Update(currentRace);
            } else if (CurrentContext.CurrentRaceTitle != currentRace.Titel) {
                return;
            } else {
                Update(currentRace);
            }
        }


        public Race CreateEmptyRace() {
            _repo.RemoveTmpFiles(DefaultRaceTitel);
            return new Race {
                Titel = DefaultRaceTitel,
                Judge = string.Empty,
                Place = string.Empty,
                RaceType = string.Empty,
                Date = DateTime.Today,
                StartTime = DateTime.Now,
                TimingTool = TimingTools.AlgeTiming,
                Groups = new List<Group>(),
            };
        }


        public void UpdateGroups(Race currentRace, Group groupToUpdate) {
            var currentGroups = currentRace.Groups.ToList();
            var oldGroup = currentGroups.Find(x => x.GroupId == groupToUpdate.GroupId);
            currentGroups[currentGroups.IndexOf(oldGroup)] = groupToUpdate;
            currentRace.Groups = currentGroups;
            Update(currentRace);
        }
    }
}

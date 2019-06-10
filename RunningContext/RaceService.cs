﻿using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunningContext {
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


        public void AddGroup(string[] input) {
            if (CurrentContext.Race == null) {
                logger.Info($"No race created yet");
                return;
            }

            var currentRaceParticipants = CurrentContext.Race.Participants.ToList();

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

            CurrentContext.Race.Participants = currentRaceParticipants;
        }


        public void AddTimingValues() {
            var race = CurrentContext.Race;
            var allGroups = race.Participants;

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


        public void CalculateFinishTimes() {
            var race = CurrentContext.Race;
            var allGroups = race.Participants;

            foreach (var group in allGroups) {
                group.TimeTaken = group.FinishTime - race.StartTime;
            }
        }


        public void Save(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"race_tmp_{CurrentContext.SaveCounter++}";
            }

            _repo.SerializeObject<Model.Race>(CurrentContext.Race, filename);
        }


        public void Load(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                return;
                //filename = $"tmp_{CurrentContext.SaveCounter++}";
            }

            var loadedScenario = _repo.DeSerializeObject<Model.Race>(filename);
            CurrentContext.Race = loadedScenario;
        }
    }
}
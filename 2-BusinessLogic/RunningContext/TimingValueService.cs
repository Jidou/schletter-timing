using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;
using SchletterTiming.ReaderInterfaces;

namespace SchletterTiming.RunningContext {
    public class TimingValueService {

        private static readonly string RacesBasePath = $"{Environment.CurrentDirectory}\\Data\\Races";

        private static uint _saveCounter = 0;
        private readonly IConfiguration _configuration;
        private readonly SaveLoad _repo;
        private readonly ITimy3Reader _timy3Reader;


        public TimingValueService(IConfiguration configuration, SaveLoad repo, ITimy3Reader timy3Reader) {
            _configuration = configuration;
            _repo = repo;
            _timy3Reader = timy3Reader;
        }


        public void SaveChangesToRaceFolder(Race race, List<TimingValue> values) {
            var filename = GetFilename(race);
            _repo.SerializeObjectFilename(values, filename);
        }


        public IEnumerable<TimingValue> LoadLatestValuesFromRaceFolder(string racename) {
            var files = _repo.GetFileList(racename);

            if (!files.Any()) {
                return new List<TimingValue>();
            }

            var latestAccesTimeFileTuple = new Tuple<DateTime, string>(DateTime.MinValue, string.Empty);

            foreach (var file in files) {
                var accessTime = File.GetLastAccessTime(file);
                if (latestAccesTimeFileTuple.Item1 < accessTime) {
                    latestAccesTimeFileTuple = new Tuple<DateTime, string>(accessTime, file);
                }
            }

            var loadedTimingValues = _repo.DeSerializeObjectFullPath<IEnumerable<TimingValue>>(latestAccesTimeFileTuple.Item2).ToList();
            return loadedTimingValues;
        }


        private void CheckIfPathIsValid(Race race) {
            if (!Directory.Exists(RacesBasePath)) {
                Directory.CreateDirectory(RacesBasePath);
            }

            if (!Directory.Exists($"{RacesBasePath}/{race.Titel}")) {
                Directory.CreateDirectory($"{RacesBasePath}/{race.Titel}");
            }
        }


        private string GetFilename(Race race) {
            CheckIfPathIsValid(race);
            return $"Races/{race.Titel}/timing_values_{_saveCounter++}";
        }


        public List<TimingValue> WaitForBulk() {
            var timingValues = _timy3Reader.WaitForBulk();
            return timingValues;
        }
    }
}

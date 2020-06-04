using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SchletterTiming.FileRepo;
using SchletterTiming.Model;

namespace SchletterTiming.RunningContext {
    public class TimingValueService {

        private static uint SaveCounter = 0;
        private readonly IConfiguration _configuration;
        private SaveLoad _repo;

        public TimingValueService(IConfiguration configuration, SaveLoad repo) {
            _configuration = configuration;
            _repo = repo;
        }


        public void Save() {
            var filename = $"timing_values_{SaveCounter++}";
            var timingValues = CurrentContext.Timing;
            _repo.SerializeObject(timingValues, filename);
        }


        public void Load(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"timing_values_{--SaveCounter}";
            }

            var loadedTimingValues = _repo.DeSerializeObject<IEnumerable<TimingValue>>(filename).ToList();
            CurrentContext.Timing = loadedTimingValues;

            //if (loadIntoRace) {
            //    CurrentContext.Race.Participants = loadedGroups;
            //} else {
            //    CurrentContext.AllAvailableGroups = loadedGroups;
            //}
        }
    }
}

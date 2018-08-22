using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningContext {
    public class TimingValue {

        private static uint SaveCounter = 0;

        public static void Save() {
            var filename = $"timing_values_{SaveCounter++}";
            var timingValues = CurrentContext.Timing;
            SaveLoad.SerializeObject(timingValues, filename);
        }


        public static void Load(string filename) {
            if (string.IsNullOrEmpty(filename)) {
                filename = $"timing_values_{--SaveCounter}";
            }

            var loadedTimingValues = SaveLoad.DeSerializeObject<IEnumerable<Model.TimingValue>>(filename).ToList();
            CurrentContext.Timing = loadedTimingValues;

            //if (loadIntoRace) {
            //    CurrentContext.Race.Participants = loadedGroups;
            //} else {
            //    CurrentContext.AllAvailableGroups = loadedGroups;
            //}
        }
    }
}

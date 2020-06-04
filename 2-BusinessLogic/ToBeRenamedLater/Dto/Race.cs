using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToBeRenamedLater.Dto {
    public class Race {

        public string RaceType { get; set; }
        public string Titel { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public string Place { get; set; }
        public string Judge { get; set; }
        public TimingTools TimingTool { get; set; }
        public IEnumerable<GroupInfoForRace> Groups { get; set; }
    }
}

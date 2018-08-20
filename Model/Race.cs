using System;
using System.Collections.Generic;

namespace Model {
    public class Race {

        public string RaceType { get; set; }
        public string Titel { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public string Judge { get; set; }
        public Timing Timing { get; set; }
        public IEnumerable<Group> Participants { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Model {
    public class Participant {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string YearOfBirth { get; set; }
        public IEnumerable<string> Category { get; set; }

        public DateTime? FinishTime { get; set; }
    }
}

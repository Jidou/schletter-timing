using System;

namespace Model {
    public class Group {
        public string Groupname { get; set; }
        public Participant Participant1 { get; set; }
        public Participant Participant2 { get; set; }
        public int Groupnumber { get; set; }
        public string Class { get; set; }

        public DateTime FinishTime { get; set; }
    }
}

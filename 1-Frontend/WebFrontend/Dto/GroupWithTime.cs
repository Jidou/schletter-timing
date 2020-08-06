using System;

namespace SchletterTiming.WebFrontend.Dto {
    public class GroupWithTime {
        public string Groupname { get; set; }
        public int Startnumber { get; set; }
        public DateTime Participant1Time { get; set; }
        public DateTime Participant2Time { get; set; }
        public DateTime FinishTime { get; set; } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Model {
    [Serializable]
    public class Group {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public int GroupId { get; set; }
        public string Groupname { get; set; }
        public Participant Participant1 { get; set; }
        public Participant Participant2 { get; set; }
        public string Class { get; set; }

        public int StartNumber { get; set; }
        public DateTime FinishTime { get; set; }
        public TimeSpan TimeTaken { get; set; }


        public Group() { }


        public Group(string groupname, int startnumber, string @class) {
            Groupname = groupname;
            StartNumber = startnumber;
            Class = @class;
        }


        public Group(string[] input) {
            Groupname = input[0];
            StartNumber = int.Parse(input[1]);
            Class = input[2];
        }
    }
}

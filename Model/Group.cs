using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Model {
    [Serializable]
    public class Group {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public string Groupname { get; set; }
        public Participant Participant1 { get; set; }
        public Participant Participant2 { get; set; }
        public int Groupnumber { get; set; }
        public string Class { get; set; }

        public DateTime FinishTime { get; set; }


        public Group() { }


        public Group(string groupname, int groupnumber, string @class) {
            Groupname = groupname;
            Groupnumber = groupnumber;
            Class = @class;
        }


        public Group(string[] input) {
            Groupname = input[0];
            Groupnumber = int.Parse(input[1]);
            Class = input[2];
        }
    }
}

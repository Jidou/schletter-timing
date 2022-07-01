using System;
using System.Collections.Generic;
using System.Text;

namespace SchletterTiming.Model {
    public class Upload {

        public string Groupname { get; set; }
        public string Class { get; set; }
        public Participant Participant1 { get; set; }
        public Participant Participant2 { get; set; }
    }
}

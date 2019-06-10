using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToBeRenamedLater.Dto {
    public class Participant {
        public int ParticipantId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string YearOfBirth { get; set; }
        public string Category { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public bool ToDelete { get; set; }
        public bool ToAdd { get; set; }
        public bool ToUpdate { get; set; }
    }
}

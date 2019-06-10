﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToBeRenamedLater.Dto {
    public class Group {
        public int GroupId { get; set; }
        public string Groupname { get; set; }
        public string Class { get; set; }
        public int Participant1Id { get; set; }
        public string Participant1FullName { get; set; }
        public int Participant2Id { get; set; }
        public string Participant2FullName { get; set; }
        public bool ToDelete { get; set; }
        public bool ToAdd { get; set; }
        public bool ToUpdate { get; set; }
    }


    public class GroupIdAndNameOnly {
        public int GroupId { get; set; }
        public string Groupname { get; set; }
    }
}

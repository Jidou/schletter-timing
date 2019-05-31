using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToBeRenamedLater.Dto {
    public class Group {
        public int GroupId { get; set; }
        public string Groupname { get; set; }
        public string Class { get; set; }
        public bool ToDelete { get; set; }
        public bool ToAdd { get; set; }
        public bool ToUpdate { get; set; }
    }
}

namespace SchletterTiming.WebFrontend.Dto {
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
        public int StartNumber { get; set; }
    }


    public class GroupIdAndNameOnly {
        public int GroupId { get; set; }
        public string Groupname { get; set; }
    }


    public class GroupInfoForRace {
        public int GroupId { get; set; }
        public string Groupname { get; set; }
        public int StartNumber { get; set; }
    }
}

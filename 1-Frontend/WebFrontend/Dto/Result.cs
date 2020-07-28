﻿using System;
using System.Collections.Generic;
using SchletterTiming.Model;

namespace SchletterTiming.WebFrontend.Dto {
    public class Result {

        public string RaceType { get; set; }
        public string Titel { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string Place { get; set; }
        public string Judge { get; set; }
        public TimingTools TimingTool { get; set; }
        public IEnumerable<GroupResult> Groups { get; set; }
    }


    public class GroupResult {
        public int GroupId { get; set; }
        public string Groupname { get; set; }
        public string Participant1Name { get; set; }
        public string Participant2Name { get; set; }
        public int Startnumber { get; set; }
        public DateTime FinishTime { get; set; }
        public string TimeTaken { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using NLog;

namespace SchletterTiming.Model {
    [Serializable]
    public class Race {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public string RaceType { get; set; }
        public string Titel { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public string Place { get; set; }
        public string Judge { get; set; }
        public TimingTools TimingTool { get; set; }
        public IEnumerable<Group> Groups { get; set; }


        public Race() { }


        public Race(string raceType, string titel, DateTime date, string place, string judge, TimingTools timingTool = TimingTools.AlgeTiming) {
            RaceType = raceType;
            Titel = titel;
            Date = date;
            Place = place;
            Judge = judge;
            TimingTool = timingTool;
            Groups = new List<Group>();
        }


        public Race(string[] input) {
            RaceType = input[0];
            Titel = input[1];
            Date = DateTime.Parse(input[2]);
            Place = input[3];
            Judge = input[4];
            TimingTool = TimingTools.AlgeTiming;
            Groups = new List<Group>();
        }
    }
}

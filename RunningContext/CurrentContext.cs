﻿using Model;
using ReaderInterfaces;
using Timy3Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningContext {
    public static class CurrentContext {

        public static ITimy3Reader Reader;

        public static int SaveCounter = 0;
        public static Model.Race Race { get; set; }
        public static List<Model.Group> AllAvailableGroups { get; set; }
        public static List<Model.Participant> AllAvailableParticipants { get; set; }
        public static List<Model.TimingValue> Timing { get; set; }
    }
}

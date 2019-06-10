using System;
using System.Linq;
using System.Collections.Generic;

namespace Model {
    [Serializable]
    public class Participant {
        public int ParticipantId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string YearOfBirth { get; set; }
        public string Category { get; set; }
        public int GroupId { get; set; }

        public DateTime? FinishTime { get; set; }


        public Participant() { }


        public Participant(string firstname, string lastname, string yearOfBirth, string category) {
            Firstname = firstname;
            Lastname = lastname;
            YearOfBirth = yearOfBirth;
            Category = category;
        }


        public Participant(string[] input) {
            Firstname = input[0];
            Lastname = input[1];
            YearOfBirth = input[2];
            Category = input[3];
        }
    }
}

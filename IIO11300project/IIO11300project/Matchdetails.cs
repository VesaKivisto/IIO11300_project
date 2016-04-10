using System.Collections.Generic;

namespace IIO11300project
{
    // A class which is a part of a larger entirety. Has some of the same data as Match class but also has lots of more comprehensive information about the played match.
    // Players, or summoners as League of Legends calls its players, are called participants in this class.
    // Match details are requested with RequestMatchDetails function defined in RiotApiHandler class.
    public class Matchdetails
    {
        public string ID { get; set; }
        public string GameMode { get; set; }
        public string CreationDate { get; set; }
        public string CreationDateSQL { get; set; }
        public string TimePlayed { get; set; }
        public string Result { get; set; }
        public List<Participant> Participants { get; set; }
        public string MatchInfo
        {
            get { return GameMode + "\nPlayed on: " + CreationDate + "\nTime played: " + TimePlayed; }
        }
    }
}
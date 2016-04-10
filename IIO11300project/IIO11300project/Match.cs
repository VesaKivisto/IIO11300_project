using System;
using System.Collections.Generic;

namespace IIO11300project
{
    // A class for displaying Match data. This specific class is used with the RequestMatchHistory function defined in the RiotApiHandler class. Matches are stored in database.
    public class Match
    {
        public string ID { get; set; }
        public string ChampionLevel { get; set; }
        public string GameMode { get; set; }
        public string GoldEarned { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int Minions { get; set; }
        public int NeutralMinions { get; set; }
        public string Result { get; set; }
        public string TimePlayed { get; set; }
        public string CreationDate { get; set; }
        // A variable for storing creation date in SQL format. Makes it easier to store the game in the database without needing to convert the date to right format.
        public string CreationDateSQL { get; set; }
        public Champion Champion { get; set; }
        public List<Item> Items { get; set; }
        public List<Spell> Spells { get; set; }
        public int CreepScore
        {
            get { return Minions + NeutralMinions; }
        }
        public string KDA
        {
            get
            {
                if (Deaths != 0)
                {
                    return Math.Round((decimal)(Kills + Assists) / Deaths, 2) + ":1 KDA";
                }
                else
                {
                    return "Perfect KDA";
                }
                
            }
        }
        public string StatsDisplay
        {
            get { return GameMode + "\n" + Kills + "/" + Deaths + "/" + Assists + "\n" + KDA; }
        }

        public string GoldDisplay
        {
            get { return GoldEarned + " gold \n" + CreepScore + " creeps" + "\n" + TimePlayed; }
        }
    }
}
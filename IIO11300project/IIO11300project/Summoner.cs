using System;

namespace IIO11300project
{
    // A class for displaying player, or summoner as League of Legends calls its players, data. Has a combination of general data as well as data from ranked games.
    // Summoner class objects are used in pretty much everywhere in this software. Summoner class is pretty much the most crucial class.
    public class Summoner
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ProfileIcon { get; set; }
        public string Region { get; set; }
        public string Level { get; set; }
        public string PlatformID { get; set; }
        public string LP { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string LeagueName { get; set; }
        public string Tier { get; set; }
        public string Division { get; set; }
        public string RankIcon { get; set; }
        public string SummonerInfo
        {
            get { return Name + "\n" + Region.ToUpper() + "\nLevel " + Level; }
        }
        public string RankedInfo
        {
            get { return Tier + " " + Division + "\n" + LeagueName + "\n" + GetLP + "\n" + Winrate; }
        }
        public string Winrate
        {
            get
            {
                if (Wins != 0 && Losses != 0)
                {
                    return Math.Round((decimal)Wins / (Wins + Losses), 2) * 100 + "% Winrate";
                }
                else
                {
                    return "";
                }
            }
        }
        public string GetLP
        {
            get
            {
                if (!String.IsNullOrEmpty(LP))
                {
                    return LP + " LP";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    public class Summoner
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ProfileIcon { get; set; }
        public string Region { get; set; }
        public string PlatformID { get; set; }
        public string LP { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string LeagueName { get; set; }
        public string Tier { get; set; }
        public string Division { get; set; }
        public string RankIcon { get; set; }
        public string TierDivision
        {
            get { return Tier + " " + Division; }
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
    }
}

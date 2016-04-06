using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    public class Match
    {
        public string ID { get; set; }
        public string ChampionID { get; set; }
        public string ChampionName { get; set; }
        public string ChampionIcon { get; set; }
        public string Spell1ID { get; set; }
        public string Spell1Icon { get; set; }
        public string Spell2Icon { get; set; }
        public string Spell2ID { get; set; }
        public string GameMode { get; set; }
        public string GoldEarned { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int CreepScore { get; set; }
        public string Result { get; set; }
        public string TimePlayed { get; set; }
        public string CreationDate { get; set; }
        public string CreationDateSQL { get; set; }
        public List<Item> Items { get; set; }
        public string KDA
        {
            get
            {
                if (Deaths != 0)
                {
                    return Math.Round((decimal)(Kills + Assists) / Deaths, 2) + " KDA";
                }
                else
                {
                    return "Perfect KDA";
                }
                
            }
        }
        public string StatsDisplay
        {
            get { return GameMode + "\n" + Kills + " / " + Deaths + " / " + Assists + "\n" + KDA + "\n" + TimePlayed + " - "  + Result; }
        }

        public string GoldDisplay
        {
            get { return GoldEarned + " gold \n" + CreepScore + " creeps"; }
        }

    }

    public class Item
    {
        public string ID { get; set; }
        public string Icon { get; set; }
    }
}

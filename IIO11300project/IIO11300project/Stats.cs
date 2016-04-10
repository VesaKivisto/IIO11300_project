using System;
using System.Collections.Generic;

namespace IIO11300project
{
    // A class to display lots of Match detail data. Is a part of the same entirety as Match details class.
    //This class is used to store and display all the needed data from a played match.
    public class Stats
    {
        public string StatsDisplay
        {
            get { return Kills + "/" + Deaths + "/" + Assists; }
        }
        public string LargestKillingSpree { get; set; }
        public string LargestMultiKill { get; set; }
        public string TowerKills { get; set; }
        public string InhibitorKills { get; set; }
        public string TotalDamageDealtToChampions { get; set; }
        public string PhysicalDamageDealtToChampions { get; set; }
        public string MagicDamageDealtToChampions { get; set; }
        public string TrueDamageDealtToChampions { get; set; }
        public string TotalDamageDealt { get; set; }
        public string PhysicalDamageDealt { get; set; }
        public string MagicDamageDealt { get; set; }
        public string TrueDamageDealt { get; set; }
        public string LargestCriticalStrike { get; set; }
        public string TotalTimeCrowdControlDealt { get; set; }
        public string TotalHeal { get; set; }
        public string TotalDamageTaken { get; set; }
        public string PhysicalDamageTaken { get; set; }
        public string MagicDamageTaken { get; set; }
        public string TrueDamageTaken { get; set; }
        public string WardsPlaced { get; set; }
        public string WardsKilled { get; set; }
        public string VisionWardsBought { get; set; }
        public string GoldEarned { get; set; }
        public string GoldSpent { get; set; }
        public int Minions { get; set; }
        public int NeutralMinions { get; set; }
        public string NeutralMinionsTeamJungle { get; set; }
        public string NeutralMinionsEnemyJungle { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public List<Item> Items { get; set; }
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
                    return Kills + "/" + Deaths + "/" + Assists + "\n" + Math.Round((decimal)(Kills + Assists) / Deaths, 2) + ":1 KDA";
                }
                else
                {
                    return Kills + "/" + Deaths + "/" + Assists + "\nPerfect KDA";
                }

            }
        }
        public string GoldDisplay
        {
            get { return GoldEarned + " gold\n" + CreepScore + " creeps"; }
        }
        
    }
}
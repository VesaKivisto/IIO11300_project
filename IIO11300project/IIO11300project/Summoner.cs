using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    public class Summoner
    {
        protected string id;
        private string name;
        private string profileIcon;
        private string region;
        private string platformID;
        private string lp;
        private int wins;
        private int losses;
        private string leagueName;
        private string tier;
        private string division;
        private string rankIcon;

        public Summoner() { }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string ProfileIcon
        {
            get { return profileIcon; }
            set { profileIcon = value; }
        }
        public string Region
        {
            get { return region; }
            set { region = value; }
        }
        public string PlatformID
        {
            get { return platformID; }
            set { platformID = value; }
        }
        public string LP
        {
            get { return lp + "LP"; }
            set { lp = value; }
        }
        public int Wins
        {
            get { return wins; }
            set { wins = value; }
        }
        public int Losses
        {
            get { return losses; }
            set { losses = value; }
        }
        public string Winrate
        {
            get
            {
                if (this.wins != 0)
                {
                    return Math.Round((decimal) wins / (wins + losses), 2) * 100 + "% Winrate";
                }
                else
                {
                    return "0% Winrate";
                }
            }
        }
        public string LeagueName
        {
            get { return leagueName; }
            set { leagueName = value; }
        }
        public string Tier
        {
            get { return tier; }
            set { tier = value; }
        }
        public string Division
        {
            get { return division; }
            set { division = value; }
        }
        public string TierDivision
        {
            get { return Tier + " " + Division; }
        }
        public string RankIcon
        {
            get { return rankIcon; }
            set { rankIcon = value; }
        }
    }
}

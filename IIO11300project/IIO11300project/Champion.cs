using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    class Champion
    {
        private string id;
        private string name;
        private string icon;
        private string level;
        private string totalPoints;
        private string pointsToNextLevel;

        public Champion()
        {
            // These are only for testing. Icon and name will be fetched from database
            this.icon = "/images/base_icons/silver.png";
            this.name = "asmfopamäf";
        }
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
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }
        public string Level
        {
            get { return level; }
            set { level = value; }
        }
        public string TotalPoints
        {
            get { return totalPoints; }
            set { totalPoints = value; }
        }
        public string PointsToNextLevel
        {
            get { return pointsToNextLevel; }
            set { pointsToNextLevel = value; }
        }
        public string ChampionDisplay
        {
            get { return Icon + " " + Name; }
        }
    }
}

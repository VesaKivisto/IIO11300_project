using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    public class Champion
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Level { get; set; }
        public string TotalPoints { get; set; }
        public string PointsToNextLevel { get; set; }
        public string ChampionDisplay
        {
            get { return Icon + " " + Name; }
        }
    }
}

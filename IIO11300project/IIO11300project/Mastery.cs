using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    public class Masterypage
    {
        public List<Mastery> masteries { get; set; }
    }
    public class Mastery
    {
        public string ID { get; set; }
        public string Icon { get; set; }
        public string Opacity { get; set; }
        public int? Rank { get; set; }
        public Dictionary<int, string> Descriptions{ get; set; }
        public string ToolTip
        {
            get
            {
                if (Rank != null)
                {
                    return Descriptions[(int)Rank];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

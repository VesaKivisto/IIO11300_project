using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIO11300project
{
    public class Runepage
    {
        public string Name { get; set; }
        public Dictionary<string, int> DescriptionCount { get; set; }
        public List<Rune> Runes { get; set; }
        public string RuneDisplay
        {
            get
            {
                string data = "";
                foreach (var key in DescriptionCount.Keys)
                {
                    if (DescriptionCount[key] != 0)
                    {
                        data += key + " x" + DescriptionCount[key].ToString() + "\n";
                    }
                    else
                    {
                        data += "";
                    }
                }
                return data;
            }
        }
    }
    public class Rune
    {
        public string ID { get; set; }
        public string SlotID { get; set; }
        public string Icon { get; set; }
        public string Descr { get; set; }
        public string Position { get; set; }
    }
}

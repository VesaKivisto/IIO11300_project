using System.Collections.Generic;

namespace IIO11300project
{
    // A class for displaying Mastery data. Masteries are things in League of Legends that give slight bonuses to player characters.
    // Mastery data is requested with RequestMasteryData function defined in RiotApiHandler class.
    public class Mastery
    {
        public string ID { get; set; }
        public string Icon { get; set; }
        public string Opacity { get; set; }
        public int? Rank { get; set; }
        // Dictionary is used to store Mastery descriptions
        // Most Masteries have several different descriptions based on Mastery rank, and descriptions for each rank are stored in this dictionary.
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

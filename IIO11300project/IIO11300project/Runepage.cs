using System.Collections.Generic;

namespace IIO11300project
{
    // A class for displaying Mastery pages. Consists of page name and a list of Masteries
    public class Runepage
    {
        public string Name { get; set; }
        // Dictionary is used to store Rune descriptions.
        // Each Rune has different description and this dictionary is basically used to store each unique description in the page
        // and how many multiples of each description (aka how many same runes) the page has.
        public Dictionary<string, int> DescriptionCount { get; set; }
        public List<Rune> Runes { get; set; }
        // Iterates through every key (= description) in the dictionary and return a string consisting of the key and the value (= how many same runes).
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
}
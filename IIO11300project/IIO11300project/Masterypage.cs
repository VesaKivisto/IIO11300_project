using System.Collections.Generic;

namespace IIO11300project
{
    // A class for displaying Mastery pages. Consists of page name and a list of Masteries
    public class Masterypage
    {
        public string Name { get; set; }
        public List<Mastery> masteries { get; set; }
    }
}
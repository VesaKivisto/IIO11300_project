using System.Collections.Generic;

namespace IIO11300project
{
    // A class to display player data. Is a part of the same entirety as Match details class.
    public class Participant
    {
        public int ID { get; set; }
        public string Team { get; set; }
        public string Role { get; set; }
        public string Lane { get; set; }
        public Champion Champion { get; set; }
        public Stats Stats { get; set; }
        public List<Spell> Spells { get; set; }
        public string RoleDisplay
        {
            get { return Role + " " + Lane; }
        }
    }
}
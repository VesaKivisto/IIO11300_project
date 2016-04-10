namespace IIO11300project
{
    // A class for displaying Rune data. Runes are things in League of Legends that give slight bonuses to player characters.
    // Rune data is requested with RequestRuneData function defined in RiotApiHandler class.
    public class Rune
    {
        public string ID { get; set; }
        public string SlotID { get; set; }
        public string Icon { get; set; }
        public string Descr { get; set; }
        // Position defines the rune placement on the rune page. Positions are fetched from a dictionary defined in BLController class based on SlotID
        public string Position { get; set; }
    }
}
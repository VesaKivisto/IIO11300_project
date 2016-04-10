namespace IIO11300project
{
    // A class for displaying Summoner spell data. Summoner spells are skills with different functionalities that can be used with any character in the game.
    // All needed Summoner spell data is stored in database and fetched there with GetSummonerSpellData defined in DBHandler
    public class Spell
    {
        public string ID { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Descr { get; set; }
        public string ToolTip
        {
            get { return Name + "\n" + Descr; }
        }
    }
}
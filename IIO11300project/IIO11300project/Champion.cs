namespace IIO11300project
{
    // A class for displaying champion data. Champions are playable characters in League of Legends.
    // Champion data is mostly stored in database and the data from there is fetched with GetChampionData function defined in DBHandler class.
    // Mastery level and points are parts of Champion masteries and these are requested with RequestChampionMastery function defined in RiotApiHandler class.
    public class Champion
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string LoadingImage { get; set; }
        public string MasteryLevel { get; set; }
        public string TotalPoints { get; set; }
        public string PointsToNextLevel { get; set; }
        public string NameTitle
        {
            get { return Name + "\n" + Title; }
        }
    }
}
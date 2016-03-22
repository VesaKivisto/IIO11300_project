using Newtonsoft.Json;

namespace IIO11300project
{
    class SummonerData
    {
        [JsonProperty("troikku")]
        public Summoner troikku { get; set; }
    }

    class Summoner
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profileIconId")]
        public int ProfileIconId { get; set; }

        [JsonProperty("revisionDate")]
        public long RevisionDate { get; set; }

        [JsonProperty("summonerLevel")]
        public int SummonerLevel { get; set; }
    }
}

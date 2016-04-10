using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace IIO11300project
{
    // All requests get data in JSON format. That JSON is assigned to a string variable which is then parsed to JObject, or in a couple exceptions to JArray.
    // All requests use WebClient with UTF8 encoding.
    public static class RiotApiHandler
    {
        // API key needed for the API requests
        private static string apiKey = "";
        // Basic API request. Gets the basic summoner (= player) data.
        public static JObject RequestSummonerData(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v1.4/summoner/by-name/" + summoner.Name + "?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JObject summonerData = JObject.Parse(json);
                return summonerData;
            }
            catch (Exception)
            {
                throw;
            } 
        }
        // API request to get summoner ranked data.
        public static JObject RequestRankedData(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v2.5/league/by-summoner/" + summoner.ID + "/entry/?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JObject rankedData = JObject.Parse(json);
                return rankedData;
            }
            catch (Exception)
            {
                JObject rankedData = new JObject();
                return rankedData;
                throw;
            } 
        }
        // API request to get summoner champion mastery data. Champion mastery is basically data which tells how much summoenr has played a certain champion.
        public static JArray RequestChampionMastery(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/championmastery/location/" + summoner.PlatformID + "/player/" + summoner.ID + "/champions?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JArray championMasteryData = JArray.Parse(json);
                return championMasteryData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // API request to get summoner mastery pages.
        public static JObject RequestMasteryPages(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v1.4/summoner/" + summoner.ID + "/masteries?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JObject masteryData = JObject.Parse(json);
                return masteryData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // API request to get summoner rune pages.
        public static JObject RequestRunePages(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v1.4/summoner/" + summoner.ID + "/runes?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JObject runeData = JObject.Parse(json);
                return runeData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // API request to get summoner's 10 latest games.
        public static JObject RequestMatchHistory(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v1.3/game/by-summoner/" + summoner.ID + "/recent?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JObject matchHistoryData = JObject.Parse(json);
                return matchHistoryData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // API request to get more comprehensive data on a certain match.
        public static JObject RequestMatchDetails(Summoner summoner, string matchID)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v2.2/match/" + matchID + "?api_key=" + apiKey;
                string json = client.DownloadString(url);
                JObject matchData = JObject.Parse(json);
                return matchData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
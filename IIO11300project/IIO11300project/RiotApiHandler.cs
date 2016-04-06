using System;
using System.Net;
using System.Windows;
using System.Web.Helpers;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace IIO11300project
{
    public static class RiotApiHandler
    {
        private static string apiKey = "";

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
                throw;
            } 
        }

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

        public static JObject RequestMatchHistory(Summoner summoner)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v1.3/game/by-summoner/" + summoner.ID + "/recent?api_key=" + apiKey;
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

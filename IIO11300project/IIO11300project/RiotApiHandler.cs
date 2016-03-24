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

namespace IIO11300project
{
    class RiotApiHandler
    {
        private string apiKey;

        public RiotApiHandler()
        {
            this.apiKey = "";
        }

        public Summoner RequestSummonerData(Summoner summoner)
        {
            try
            {
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v1.4/summoner/by-name/" + summoner.Name + "?api_key=" + this.apiKey;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(url);
                JObject summonerData = JObject.Parse(json);
                summoner.ID = summonerData[summoner.Name]["id"].ToString();
                summoner.ProfileIcon = GetProfileIconURL(summonerData[summoner.Name]["profileIconId"].ToString());
                summoner.Name = summonerData[summoner.Name]["name"].ToString();
                return summoner;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Summoner RequestSummonerRankedData(Summoner summoner)
        {
            try
            {
                string url = "https://" + summoner.Region + ".api.pvp.net/api/lol/" + summoner.Region + "/v2.5/league/by-summoner/" + summoner.ID + "/entry/?api_key=" + this.apiKey;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(url);
                JObject summonerRankedData = JObject.Parse(json);
                int temp;
                bool parsed;

                if (summonerRankedData.ToString() != "{}")
                {
                    parsed = int.TryParse(summonerRankedData[summoner.ID][0]["entries"][0]["wins"].ToString(), out temp);
                    if (parsed)
                    {
                        summoner.Wins = temp;
                    }
                    else
                    {
                        MessageBox.Show("Cannot parse given value to integer!");
                    }

                    parsed = int.TryParse(summonerRankedData[summoner.ID][0]["entries"][0]["losses"].ToString(), out temp);
                    if (parsed)
                    {
                        summoner.Losses = temp;
                    }
                    else
                    {
                        MessageBox.Show("Cannot parse given value to integer!");
                    }

                    summoner.LP = summonerRankedData[summoner.ID][0]["entries"][0]["leaguePoints"].ToString();
                    summoner.LeagueName = summonerRankedData[summoner.ID][0]["name"].ToString();
                    summoner.Tier = summonerRankedData[summoner.ID][0]["tier"].ToString();

                    if (summoner.Tier != "CHALLENGER" && summoner.Tier != "MASTER")
                    {
                        summoner.Division = summonerRankedData[summoner.ID][0]["entries"][0]["division"].ToString();
                        summoner.RankIcon = "/images/tier_icons/" + summoner.Tier.ToLower() + "_" + summoner.Division.ToLower() + ".png";
                    }
                    else
                    {
                        summoner.Division = "";
                        summoner.RankIcon = "/images/base_icons/" + summoner.Tier.ToLower() + ".png";
                    }
                }
                else
                {
                    summoner.RankIcon = "/images/base_icons/provisional.png";
                }

                return summoner;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return summoner;
                    }
                }
                return summoner;
            }
        }
        
        public List<Champion> RequestChampionMastery(Summoner summoner)
        {
            try
            {
                List<Champion> champions = new List<Champion>();
                WebClient client = new WebClient();

                client.Encoding = Encoding.UTF8;
                string url = "https://" + summoner.Region + ".api.pvp.net/championmastery/location/" + summoner.PlatformID + "/player/" + summoner.ID + "/champions?api_key=" + this.apiKey;
                string json = client.DownloadString(url);
                JArray championMasteryData = JArray.Parse(json);

                foreach (var value in championMasteryData)
                {
                    Champion champion = new Champion();
                    champion.ID = value["championId"].ToString();
                    champion.Level = value["championLevel"].ToString();
                    champion.TotalPoints = value["championPoints"].ToString();
                    champion.PointsToNextLevel = value["championPointsUntilNextLevel"].ToString();
                    champions.Add(champion);
                }

                return champions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, string> GetRegionsPlatforms()
        {
            Dictionary<string, string> regions = new Dictionary<string, string>();
            regions.Add("BR", "BR1");
            regions.Add("EUNE", "EUN1");
            regions.Add("EUW", "EUW1");
            regions.Add("KR", "KR");
            regions.Add("LAN", "LA1");
            regions.Add("LAS", "LA2");
            regions.Add("NA", "NA1");
            regions.Add("OCE", "OC1");
            regions.Add("RU", "RU");
            regions.Add("TR", "TR1");
            return regions;
        }

        public string GetPlatformByRegion(string region)
        {
            Dictionary<string, string> regions = GetRegionsPlatforms();
            string platformID = regions[region.ToLower()];
            return platformID;
        }

        // This is going to get replaced
        public string GetProfileIconURL(string profileIcon)
        {
            string url = "http://ddragon.leagueoflegends.com/cdn/6.5.1/img/profileicon/" + profileIcon + ".png";
            return url;
        }
    }
}

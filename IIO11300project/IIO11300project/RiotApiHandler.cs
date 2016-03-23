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
        private string region;
        private string summonerName;
        private string summonerID;

        public RiotApiHandler()
        {
            this.apiKey = "";
        }

        public JObject RequestSummonerData(string region, string summonerName)
        {
            this.region = region;
            this.summonerName = summonerName;
            string url = "https://" + this.region + ".api.pvp.net/api/lol/" + this.region + "/v1.4/summoner/by-name/" + this.summonerName + "?api_key=" + this.apiKey;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(url);

            // TÄMÄ TOIMII, WAUUUUUU
            JObject summonerData = JObject.Parse(json);
            return summonerData;

            /*
            string id = o[this.summonername]["id"].ToString();
            string name = o[this.summonername]["name"].ToString();
            string profileIcon = o[this.summonername]["profileIconId"].ToString();

            MessageBox.Show("ID: " + id + "\n\rName: " + name + "\n\rIcon: " + profileIcon);
            */
        }

        public string GetProfileIconURL(string profileIcon)
        {
            string url = "http://ddragon.leagueoflegends.com/cdn/6.5.1/img/profileicon/" + profileIcon + ".png";
            return url;
        }

        public JObject RequestSummonerRankedData(string region, string summonerID)
        {
            this.region = region;
            this.summonerID = summonerID;
            string url = "https://" + this.region + ".api.pvp.net/api/lol/" + this.region + "/v2.5/league/by-summoner/" + this.summonerID + "/entry/?api_key=" + this.apiKey;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            try
            {
                string json = client.DownloadString(url);

                JObject summonerRankedData = JObject.Parse(json);
                return summonerRankedData;
            }
            catch (WebException ex)
            {
                JObject summonerRankedData = new JObject();
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return summonerRankedData;
                    }
                }
                return summonerRankedData;
            }
        }
    }
}

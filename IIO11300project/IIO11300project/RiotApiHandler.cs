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
        private string summonername;

        public RiotApiHandler()
        {
            this.apiKey = "b4772fd0-7fdc-4b50-a7bd-e9b4a2c8f61d";
        }

        public void RequestSummonerData(string region, string summonername)
        {
            this.region = region.ToLower();
            this.summonername = summonername.ToLower();
            string url = "https://" + this.region + ".api.pvp.net/api/lol/" + this.region + "/v1.4/summoner/by-name/" + this.summonername + "?api_key=" + this.apiKey;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string json = client.DownloadString(url);

            //SummonerData summoner = JsonConvert.DeserializeObject<SummonerData>(json);
            //MessageBox.Show(summoner.troikku.Name);

            // TÄMÄ TOIMII, WAUUUUUU
            JObject o = JObject.Parse(json);
            string id = o[this.summonername]["id"].ToString();
            string name = o[this.summonername]["name"].ToString();
            string profileIcon = o[this.summonername]["profileIconId"].ToString();

            MessageBox.Show("ID: " + id + "\n\rName: " + name + "\n\rIcon: " + profileIcon);
        }
    }
}

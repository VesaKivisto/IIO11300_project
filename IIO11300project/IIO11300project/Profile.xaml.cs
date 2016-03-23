using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile(Summoner summoner)
        {
            InitializeComponent();
            InitSummonerProfile(summoner);
        }

        public void InitSummonerProfile(Summoner summoner)
        {
            JObject summonerRankedData;
            RiotApiHandler apiHandler = new RiotApiHandler();
            int temp;
            bool parsed;
            summonerRankedData = apiHandler.RequestSummonerRankedData(summoner.Region, summoner.ID.ToString());

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

            imgProfileIcon.Source = new BitmapImage(new Uri(apiHandler.GetProfileIconURL(summoner.ProfileIcon)));
            tbSummonerName.Text = summoner.Name;
            tbSummonerRegion.Text = summoner.Region.ToUpper();
            imgRankIcon.Source = new BitmapImage(new Uri(summoner.RankIcon, UriKind.Relative));
            tbRank.Text = summoner.Tier + " " + summoner.Division;
            tbLeagueName.Text = summoner.LeagueName;
            tbRankLP.Text = summoner.LP;
            if (summoner.Wins != 0 && summoner.Losses != 0)
            {
                tbRankWinrate.Text = summoner.Winrate.ToString();
            }
        }
    }
}

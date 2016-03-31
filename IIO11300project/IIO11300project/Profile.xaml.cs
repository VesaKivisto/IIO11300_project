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
        Summoner summoner;
        public Profile(Summoner summoner)
        {
            this.summoner = summoner;
            InitializeComponent();
            InitSummonerProfile(summoner);
        }

        public void InitSummonerProfile(Summoner summoner)
        {
            summoner = RiotApiHandler.RequestRankedData(summoner);
            spSummonerInfo.DataContext = summoner;
        }

        private void tcMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabControl tabControl = (TabControl)sender;
                TabItem currentTab = (TabItem)tabControl.SelectedItem;
                string current = (string)currentTab.Header;
                switch(current)
                {
                    case "Champions":
                        List<Champion> champions = RiotApiHandler.RequestChampionMastery(summoner);
                        dgChampions.DataContext = champions;
                        break;
                    case "Masteries":
                        /*
                        List<string> masteries = RiotApiHandler.RequestMasteryPages(summoner);
                        grdMasteryPage.DataContext = masteries;
                        */
                        List<Mastery> masteries = RiotApiHandler.GetAllMasteryData();
                        masteries = RiotApiHandler.RequestMasteryPages(summoner, masteries);
                        grdMasteryPage.DataContext = masteries;
                        break;
                    case "Runes":
                        List<string> runes = RiotApiHandler.RequestRunePages(summoner);
                        grdRunePage.DataContext = runes;
                        break;
                }
            }
        }
    }
}

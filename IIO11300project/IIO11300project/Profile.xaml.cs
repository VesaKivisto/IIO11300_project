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
        List<Match> matches = new List<Match>();
        List<Champion> champions = new List<Champion>();
        List<Masterypage> masteryPages = new List<Masterypage>();
        List<Runepage> runePages = new List<Runepage>();
        public Profile(Summoner summoner)
        {
            this.summoner = summoner;
            InitializeComponent();
            InitSummonerProfile(summoner);
        }

        public void InitSummonerProfile(Summoner summoner)
        {
            summoner = BLController.GetRankedData(summoner);
            spSummonerInfo.DataContext = summoner;
        }

        private void tcMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabControl tabControl = (TabControl)sender;
                TabItem currentTab = (TabItem)tabControl.SelectedItem;
                string current = (string)currentTab.Header;
                switch (current)
                {
                    case "Matches":
                        try
                        {
                            matches = BLController.GetMatchHistory(summoner, matches);
                            dgMatches.DataContext = matches;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case "Champions":
                        try
                        {
                            champions = BLController.GetChampionMastery(summoner, champions);
                            dgChampions.DataContext = champions;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case "Masteries":
                        try
                        {
                            masteryPages = BLController.GetMasteryPages(summoner, masteryPages);
                            grdMasteryPage.DataContext = masteryPages[0].masteries;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case "Runes":
                        try
                        {
                            runePages = BLController.GetRunePages(summoner, runePages);
                            grdRunes.DataContext = runePages[0].Runes;
                            spPageInfo.DataContext = runePages[0];
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                }
            }
        }
    }
}

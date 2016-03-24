﻿using Newtonsoft.Json.Linq;
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
            RiotApiHandler apiHandler = new RiotApiHandler();
            summoner = apiHandler.RequestSummonerRankedData(summoner);
            spSummonerInfo.DataContext = summoner;
        }

        private void tcMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RiotApiHandler apiHandler = new RiotApiHandler();
            if (e.Source is TabControl)
            {
                TabControl tabControl = (TabControl)sender;
                TabItem currentTab = (TabItem)tabControl.SelectedItem;
                string current = (string)currentTab.Header;
                switch(current)
                {
                    case "Champions":
                        summoner.PlatformID = apiHandler.GetPlatformByRegion(summoner.Region).ToLower();
                        GetChampionMastery(summoner.PlatformID);
                        break;
                }
            }
        }

        private void GetChampionMastery(string platformID)
        {
            try
            {
                List<Champion> champions = new List<Champion>();
                RiotApiHandler apiHandler = new RiotApiHandler();
                champions = apiHandler.RequestChampionMastery(summoner);
                dgChampions.DataContext = champions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

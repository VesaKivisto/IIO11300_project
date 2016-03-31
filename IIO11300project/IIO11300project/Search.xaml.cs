using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        RiotApiHandler apiHandler;
        public Search()
        {
            InitializeComponent();
            InitCombobox();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                apiHandler = new RiotApiHandler();
                Summoner summoner = new Summoner();
                summoner.Name = txtSummonerName.Text.ToLower();
                summoner.Region = cbRegions.SelectedValue.ToString().ToLower();
                summoner = apiHandler.RequestSummonerData(summoner);

                Profile profile = new Profile(summoner);
                profile.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitCombobox()
        {
            apiHandler = new RiotApiHandler();
            Dictionary<string, string> regions = new Dictionary<string, string>();

            regions = apiHandler.GetRegionsPlatforms();
            foreach (var value in regions.Keys)
            {
                cbRegions.Items.Add(value);
            }

            cbRegions.SelectedIndex = 2;
            
        }
    }
}

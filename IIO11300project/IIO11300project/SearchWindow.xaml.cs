using System;
using System.Collections.Generic;
using System.Windows;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            InitCombobox();
        }
        // Requests the summoner data. If no errors are given opens a profile window and closes the search window.
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Summoner summoner = new Summoner();
                summoner.Name = txtSummonerName.Text.ToLower();
                summoner.Region = cbRegions.SelectedValue.ToString().ToLower();
                summoner = BLController.GetSummonerData(summoner);

                ProfileWindow profile = new ProfileWindow(summoner);
                profile.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Fills the combobox with all possible regions the summoner can play on.
        private void InitCombobox()
        {
            Dictionary<string, string> regions = new Dictionary<string, string>();
            regions = BLController.GetRegionsPlatforms();
            foreach (var value in regions.Keys)
            {
                cbRegions.Items.Add(value);
            }
        }
    }
}
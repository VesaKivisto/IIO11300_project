using Newtonsoft.Json.Linq;
using System.Windows;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitCombobox();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            JObject summonerData;
            RiotApiHandler apiHandler = new RiotApiHandler();
            Summoner summoner = new Summoner();

            string region = cbRegions.SelectedValue.ToString().ToLower();
            string summonerName = txtSummonerName.Text.ToLower();
            summonerData = apiHandler.RequestSummonerData(region, summonerName);
            summoner.ID = summonerData[summonerName]["id"].ToString();
            summoner.Name = summonerData[summonerName]["name"].ToString();
            summoner.ProfileIcon = summonerData[summonerName]["profileIconId"].ToString();

            Profile profile = new Profile(summoner);

            /*
            string id = o[this.summonername]["id"].ToString();
            string name = o[this.summonername]["name"].ToString();
            string profileIcon = o[this.summonername]["profileIconId"].ToString();
            */
        }

        private void InitCombobox()
        {
            string[] regions = new string[10] { "BR", "EUNE", "EUW", "KR", "LAN", "LAS", "NA", "OCE", "RU", "TR" };
            cbRegions.ItemsSource = regions;
        }
    }
}

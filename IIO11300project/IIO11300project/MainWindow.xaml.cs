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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            RiotApiHandler apiHandler = new RiotApiHandler();
            string region = cbRegions.SelectedValue.ToString();
            string summonername = txtSummonername.Text;
            apiHandler.RequestSummonerData(region, summonername);
        }

        private void InitCombobox()
        {
            string[] regions = new string[10] { "BR", "EUNE", "EUW", "KR", "LAN", "LAS", "NA", "OCE", "RU", "TR" };
            cbRegions.ItemsSource = regions;
        }
    }
}

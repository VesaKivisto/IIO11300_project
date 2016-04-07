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
        bool runes = false;
        bool masteries = false;
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
                            grdMasteries.DataContext = masteryPages[0].masteries;
                            if (masteries == false)
                            {
                                GeneratePages(masteryPages);
                                masteries = true;
                            }
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
                            if (runes == false)
                            {
                                GeneratePages(runePages);
                                runes = true;
                            } 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                        
                }
            }
        }

        private void Rune_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label current = (Label)sender;
            int index;
            int temp;
            bool parsed;
            parsed = int.TryParse(current.Content.ToString(), out temp);
            if (parsed)
            {
                index = temp - 1;
                grdRunes.DataContext = runePages[index].Runes;
                spPageInfo.DataContext = runePages[index];
            }
            else
            {
                MessageBox.Show("Cannot parse given value to integer!");
            }
        }

        private void Mastery_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label current = (Label)sender;
            int index;
            int temp;
            bool parsed;
            parsed = int.TryParse(current.Content.ToString(), out temp);
            if (parsed)
            {
                index = temp - 1;
                grdMasteries.DataContext = masteryPages[index].masteries;
            }
            else
            {
                MessageBox.Show("Cannot parse given value to integer!");
            }
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label current = (Label)sender;
            current.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label current = (Label)sender;
            current.Background = new SolidColorBrush(Colors.White);
        }

        private void GeneratePages<T>(List<T> pages)
        {
            string type = pages.GetType().GetGenericArguments().Single().ToString();
            int count = pages.Count();
            int marginLeft = 0;
            int marginRight = 568;
            for (int index = 0; index < count; index++)
            {
                Label label = new Label();
                label.Content = (index + 1).ToString();
                label.BorderThickness = new Thickness(1);
                label.BorderBrush = new SolidColorBrush(Colors.Black);
                label.Padding = new Thickness(0);
                label.Margin = new Thickness(marginLeft, 0, marginRight, 0);
                label.Height = 19;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                if (type == "IIO11300project.Masterypage")
                {
                    label.MouseLeftButtonUp += Mastery_MouseLeftButtonUp;
                    grdMasteryPages.Children.Add(label);
                }
                else if (type == "IIO11300project.Runepage")
                {
                    label.MouseLeftButtonUp += Rune_MouseLeftButtonUp;
                    grdRunePages.Children.Add(label);
                }
                marginLeft += 30;
                marginRight -= 30;
            }
        }
    }
}

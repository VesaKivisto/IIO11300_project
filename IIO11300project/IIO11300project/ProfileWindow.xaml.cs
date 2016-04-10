using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        // Some variables that are needed. Booleans are used to check if rune and mastery pages are already requested.
        Summoner summoner;
        List<Match> matches = new List<Match>();
        List<Champion> champions = new List<Champion>();
        List<Masterypage> masteryPages = new List<Masterypage>();
        List<Runepage> runePages = new List<Runepage>();
        bool runes = false;
        bool masteries = false;

        public ProfileWindow(Summoner summoner)
        {
            InitializeComponent();
            this.summoner = summoner;
            InitSummonerProfile(this.summoner);
        }
        // Requests summoner ranked data. Sets the datacontext if no errors are given.
        public void InitSummonerProfile(Summoner summoner)
        {
            try
            {
                summoner = BLController.GetRankedData(summoner);
                spSummonerInfo.DataContext = summoner;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Event handler for tabcontrol selections.
        private void tcMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                TabControl tabControl = (TabControl)sender;
                TabItem currentTab = (TabItem)tabControl.SelectedItem;
                string current = (string)currentTab.Header;
                switch (current)
                {
                    // Case for match history. Requests match history and sets datacontext if no errors are given.
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
                    // Case for champion mastery. Requests champion mastery and sets datacontext if no errors are given.
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
                    // Case for mastery pages. Requests mastery pages and sets datacontext if no errors are given.
                    case "Mastery pages":
                        try
                        {
                            masteryPages = BLController.GetMasteryPages(summoner, masteryPages);
                            grdMasteries.DataContext = masteryPages[0].masteries;
                            txtName.DataContext = masteryPages[0].Name;
                            txtMasteryPageNumber.Text = "Page 1";
                            // Execute GeneratePages function if it hasn't been executed yet.
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
                    // Case for rune pages. Requests rune pages and sets datacontext if no errors are given.
                    case "Rune pages":
                        try
                        {
                            runePages = BLController.GetRunePages(summoner, runePages);
                            grdRunes.DataContext = runePages[0].Runes;
                            spPageInfo.DataContext = runePages[0];
                            txtRunePageNumber.Text = "Page 1";
                            // Execute GeneratePages function if it hasn't been executed yet.
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
        // Event handler for rune page mouse left button. Sets the datacontext to corresponding rune page.
        // I tried to combine this with mastery page but couldn't come up with anything nice.
        private void Rune_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label current = (Label)sender;
            int index;
            int temp;
            bool parsed;
            parsed = int.TryParse(current.Content.ToString(), out temp);
            if (parsed)
            {
                txtRunePageNumber.Text = "Page " + temp.ToString();
                index = temp - 1;
                grdRunes.DataContext = runePages[index].Runes;
                spPageInfo.DataContext = runePages[index];
            }
            else
            {
                MessageBox.Show("Cannot parse given value to integer!");
            }
        }
        // Event handler for mastery page mouse left button. Sets the datacontext to corresponding rune page.
        // I tried to combine this with rune page but couldn't come up with anything nice.
        private void Mastery_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label current = (Label)sender;
            int index;
            int temp;
            bool parsed;
            parsed = int.TryParse(current.Content.ToString(), out temp);
            if (parsed)
            {
                txtMasteryPageNumber.Text = "Page " + temp.ToString();
                index = temp - 1;
                grdMasteries.DataContext = masteryPages[index].masteries;
                txtName.DataContext = masteryPages[index].Name;
            }
            else
            {
                MessageBox.Show("Cannot parse given value to integer!");
            }
        }
        // Event handler for label mouse enter. Changes background color to gray.
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label current = (Label)sender;
            current.Background = new SolidColorBrush(Colors.LightGray);
        }
        // Event handler for label mouse leave. Changes background color back to white.
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label current = (Label)sender;
            current.Background = new SolidColorBrush(Colors.White);
        }
        // A function used to generate page labels. Uses generic List, so it works with both mastery and rune pages.
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
                // Checks the type of given List to determine what to do.
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
        // Event handler to handle match detail selections. Selected match will open a match detail window.
        private void dgMatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int index = dgMatches.SelectedIndex;
                Matchdetails details = BLController.GetMatchDetails(summoner, matches[index]);
                MatchDetailsWindow detailsWindow = new MatchDetailsWindow(details);
                detailsWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for MatchDetailsWindow.xaml
    /// </summary>
    public partial class MatchDetailsWindow : Window
    {
        Matchdetails details;
        public MatchDetailsWindow(Matchdetails details)
        {
            // Mostly just initializing things.
            InitializeComponent();
            this.details = details;
            GenerateData(details);
            grdData.DataContext = details;
            GenerateChampionIcons(details);
        }
        // Creates a champion icon for every participant with their respective champions.
        public void GenerateChampionIcons(Matchdetails details)
        {
            int count = 0;
            int marginLeft = 18;
            foreach (var summoner in details.Participants)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(summoner.Champion.Icon));
                image.Height = 40;
                image.Width = 40;
                image.Margin = new Thickness(marginLeft, 0, marginLeft, 0);
                image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
                image.Name = String.Format("image{0}", count.ToString());
                image.ToolTip = summoner.Champion.Name;
                spIcons.Children.Add(image);
                count++;
            }
        }
        // Event handler for image mouse left button. Each image opens a new summoner details window when clicked.
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image current = (Image)sender;
            string temp = current.Name;
            string output = temp.Substring(temp.Length - 1, 1);
            int index = int.Parse(output);
            Participant participant = new Participant();
            participant = details.Participants[index];
            SummonerDetailsWindow summonerDetails = new SummonerDetailsWindow(participant);
            summonerDetails.Show();
        }
        // Generates match data for each participant. Basically just lots of textblock with certain data binding.
        private void GenerateData(Matchdetails details)
        {
            // index variable is used to have dynamic names for stackpanels. Each participant (= summoner) has his/her own stackpanel.
            int index = 0;
            foreach (var summoner in details.Participants)
            {
                int propCount = 0;
                foreach (var prop in summoner.Stats.GetType().GetProperties())
                {
                    TextBlock txt = new TextBlock();
                    Binding binding = new Binding();
                    Separator separator = new Separator();
                    var indexText = index.ToString(CultureInfo.InvariantCulture);
                    var stackPanelID = "spSummoner" + indexText;
                    // Finds the corresponding stackpanel with the stackPanelID.
                    var stackPanel = (StackPanel)grdData.FindName(stackPanelID);
                    string property = prop.Name;
                    binding.Path = new PropertyPath(String.Format("Participants[{0}].Stats.{1}", index, property));
                    BindingOperations.SetBinding(txt, TextBlock.TextProperty, binding);
                    txt.TextAlignment = TextAlignment.Center;
                    stackPanel.Children.Add(txt);
                    stackPanel.Children.Add(separator);
                    propCount++;
                    // Breaks the loop when property count is 29. Otherwise this would show data that isn't needed.
                    if (propCount == 29)
                    {
                        break;
                    }
                }
                index++;
            }
        }
    }
}
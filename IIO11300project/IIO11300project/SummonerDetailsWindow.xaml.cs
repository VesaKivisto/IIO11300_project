using System.Windows;

namespace IIO11300project
{
    /// <summary>
    /// Interaction logic for SummonerDetailsWindow.xaml
    /// </summary>
    public partial class SummonerDetailsWindow : Window
    {
        // Here doesn't happen much. Besides regular component initialization there's only setting participant as datagrid context.
        Participant participant;
        public SummonerDetailsWindow(Participant participant)
        {
            InitializeComponent();
            this.participant = participant;
            grdData.DataContext = participant;
        }
    }
}
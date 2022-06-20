using System.Windows;
using System.Windows.Controls;
using LSBEncoding.Views.DecoderView;
using LSBEncoding.Views.EncoderView;

namespace LSBEncoding.Views.MainWindow
{
    public partial class MainWindowView : Window
    {
        private EncoderViewModel EncoderPage;
        private DecoderViewModel DecoderPage;
        public MainWindowView()
        {
            InitializeComponent();
            DecoderPage = new DecoderViewModel();
            EncoderPage = new EncoderViewModel();
        }
        private void openMenuButton_Click(object sender, RoutedEventArgs e)
        {
            closeMenuButton.Visibility = Visibility.Visible;
            openMenuButton.Visibility = Visibility.Collapsed;
        }

        private void closeMenuButton_Click(object sender, RoutedEventArgs e)
        {
            closeMenuButton.Visibility = Visibility.Collapsed;
            openMenuButton.Visibility = Visibility.Visible;
        }

        private void listViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "encoderListViewItem":
                    mainFrame.Content = EncoderPage;
                    break;
                case "decoderListViewItem":
                    mainFrame.Content = DecoderPage;
                    break;
                default:
                    break;
            }
        }

    }

}

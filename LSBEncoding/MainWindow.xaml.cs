using System.Windows;
using System.Windows.Controls;
using LSBEncoding.Pages;

namespace LSBEncoding
{
    public partial class MainWindow : Window
    {
        private EncoderPage EncoderPage;
        private DecoderPage DecoderPage;
        public MainWindow()
        {
            InitializeComponent();
            DecoderPage = new DecoderPage();
            EncoderPage = new EncoderPage();
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

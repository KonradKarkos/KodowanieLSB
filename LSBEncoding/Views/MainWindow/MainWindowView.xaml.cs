using System.Windows;

namespace LSBEncoding.Views.MainWindow
{
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
    }

}

using LSBEncoding.Views.MainWindow;
using System.Windows;

namespace LSBEncoding
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindowView().Show();
        }

    }
}

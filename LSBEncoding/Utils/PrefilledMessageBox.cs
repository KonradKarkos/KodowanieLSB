using System.Windows;

namespace LSBEncoding.Utils
{
    public static class PrefilledMessageBox
    {
        public static void ShowInformation(string information, string title = "Success!")
        {
            MessageBox.Show(information, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowError(string information, string title = "Error!")
        {
            MessageBox.Show(information, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

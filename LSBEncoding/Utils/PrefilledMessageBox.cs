using System.Windows;

namespace LSBEncoding.Utils
{
    /// <summary>
    /// Class for easier usage of <see cref="MessageBox"/> notifications
    /// </summary>
    public static class PrefilledMessageBox
    {
        /// <summary>
        /// Shows information <see cref="MessageBox"/>
        /// </summary>
        /// <param name="information">Information to be displayed</param>
        public static void ShowInformation(string information)
        {
            MessageBox.Show(information, "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Shows error <see cref="MessageBox"/>
        /// </summary>
        /// <param name="information">Information to be displayed</param>
        public static void ShowError(string information)
        {
            MessageBox.Show(information, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

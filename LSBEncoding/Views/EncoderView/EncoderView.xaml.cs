using System.Windows.Controls;

namespace LSBEncoding.Views.EncoderView
{
    public partial class EncoderView : UserControl
    {
        public EncoderView()
        {
            InitializeComponent();
        }

        private void toEncodeANDSaveEncodedInImageFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (toEncodeInImageFilePathTextBox.Text.Length > 0 && saveEncodedImageFilePathTextBox.Text.Length > 0) encodeButton.IsEnabled = true;
            else encodeButton.IsEnabled = false;
        }

    }
}

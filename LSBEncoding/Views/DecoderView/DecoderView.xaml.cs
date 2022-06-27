using Microsoft.Win32;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace LSBEncoding.Views.DecoderView
{
    public partial class DecoderView : UserControl
    {
        public DecoderView()
        {
            InitializeComponent();
        }

        private void imageToDecodeFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (imageToDecodeFilePathTextBox.Text.Length > 0) decodeButton.IsEnabled = true;
            else decodeButton.IsEnabled = false;
        }

        private void decodedStringSaveFileFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decodedStringSaveFileFilePathTextBox.Text.Length > 0) saveDecodedStringToFileButton.IsEnabled = true;
            else saveDecodedStringToFileButton.IsEnabled = false;
        }

    }
}

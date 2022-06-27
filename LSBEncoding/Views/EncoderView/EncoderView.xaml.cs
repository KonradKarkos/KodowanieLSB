using Microsoft.Win32;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Color = System.Drawing.Color;

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

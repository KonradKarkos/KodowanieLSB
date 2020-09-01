using Microsoft.Win32;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace LSBEncoding.Pages
{
    public partial class DecoderPage : Page
    {
        public DecoderPage()
        {
            InitializeComponent();
        }
        public void SelectFile(TextBox textBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp|Tagged Image File Format (*.tif)|*.tif|Joint Photographic Experts Group (*.jpg)|*.jpg|Graphic Interchange Format (*.gif)|*.gif|Portable Network Graphics (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = openFileDialog.FileName;
            }
        }
        private void imageToDecodeChooseFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(imageToDecodeFilePathTextBox);
            decodedStringSaveFileFilePathTextBox.Text = imageToDecodeFilePathTextBox.Text.Remove(imageToDecodeFilePathTextBox.Text.Length - 4, 4).Insert(imageToDecodeFilePathTextBox.Text.Length - 4, ".txt");
        }
        private void decodedStringSaveFileChooseFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                decodedStringSaveFileFilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void decodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(imageToDecodeFilePathTextBox.Text))
            {
                int readBitsToDecode = ((ComboBoxItem)LSBNumberComboBox.SelectedItem).Content.ToString()[0] - '0';
                Bitmap imageToDecode = new Bitmap(imageToDecodeFilePathTextBox.Text);
                int imageWidth = imageToDecode.Width;
                int imageHeight = imageToDecode.Height;
                Color pixelToDecode;
                BitArray[] encodedPixelColorRGBArrays = new BitArray[3];
                BitArray decodedBits = new BitArray(imageWidth * imageHeight * readBitsToDecode * 3);
                int bitsToDecodeIndex = 0;
                for (int i = 0; i < imageWidth; i++)
                {
                    for (int j = 0; j < imageHeight; j++)
                    {
                        pixelToDecode = imageToDecode.GetPixel(i, j);
                        encodedPixelColorRGBArrays[0] = new BitArray(new byte[] { pixelToDecode.R });
                        encodedPixelColorRGBArrays[1] = new BitArray(new byte[] { pixelToDecode.G });
                        encodedPixelColorRGBArrays[2] = new BitArray(new byte[] { pixelToDecode.B });
                        for (int z = 0; z < 3; z++)
                        {
                            for (int u = 7 - (readBitsToDecode - 1); u < 8; u++)
                            {
                                decodedBits[bitsToDecodeIndex] = encodedPixelColorRGBArrays[z][u];
                                bitsToDecodeIndex++;
                            }
                        }
                    }
                }
                Byte[] LiteryWBajtach = new Byte[decodedBits.Length / 8];
                BitArray Litera;
                for (int i = 0; i < LiteryWBajtach.Length; i++)
                {
                    Litera = new BitArray(new bool[] { decodedBits[i * 8], decodedBits[i * 8 + 1], decodedBits[i * 8 + 2], decodedBits[i * 8 + 3], decodedBits[i * 8 + 4], decodedBits[i * 8 + 5], decodedBits[i * 8 + 6], decodedBits[i * 8 + 7] });
                    Litera.CopyTo(LiteryWBajtach, i);
                }
                decodedStringTextBox.Text = Encoding.ASCII.GetString(LiteryWBajtach);
                MessageBox.Show("Decoding finished");
                imageToDecode.Dispose();
            }
            else
            {
                MessageBox.Show("Image with text to decode does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void saveDecodedStringToFileButton_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter(decodedStringSaveFileFilePathTextBox.Text);
            if (decodedStringTextBox.LineCount > 0)
            {
                for (int i = 0; i < decodedStringTextBox.LineCount; i++)
                {
                    streamWriter.Write(decodedStringTextBox.GetLineText(i));
                }
                streamWriter.Dispose();
            }
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

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Application uses Least Sginificant Bit to encode and decode text in/from images converted into bitmap.\n" +
                "Encoder swaps choosen number of bits of every RGB component of color with text converted to bitarray till all text bits have been swapped - the more bits of every RGB component have been swapped the more will resulting image differ from original one.\n" +
                "Decoder reads message by reading choosen number of bits from every pixel RGB components and converting them into text.\n" +
                "All characters outside of ASCII will be changed into most corresponding ASCII characters.");
        }
    }
}

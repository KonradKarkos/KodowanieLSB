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

namespace LSBEncoding.Pages.EncoderPage
{
    public partial class EncoderPageView : Page
    {
        public EncoderPageView()
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
        private void toEncodeInImageChooseFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(toEncodeInImageFilePathTextBox);
            saveEncodedImageFilePathTextBox.Text = toEncodeInImageFilePathTextBox.Text.Insert(toEncodeInImageFilePathTextBox.Text.Length - 4, "Encoded");
        }

        private void saveEncodedImageChooseFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            SelectFile(saveEncodedImageFilePathTextBox);
        }
        private void loadStringToEncodeFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            String Plik;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                Plik = openFileDialog.FileName;
                StreamReader streamReader = new StreamReader(Plik, Encoding.GetEncoding(1250));
                StringBuilder stringBuilder = new StringBuilder();
                String loadedLine;
                while ((loadedLine = streamReader.ReadLine()) != null)
                {
                    stringBuilder.Append(loadedLine);
                }
                stringToEncodeTextBox.Text = stringBuilder.ToString();
                streamReader.Dispose();
            }
        }
        private String ReplaceSpecialLetters(String strMessage)
        {
            strMessage = Regex.Replace(strMessage, "[éèëêðę]", "e");
            strMessage = Regex.Replace(strMessage, "[ÉÈËÊĘ]", "E");
            strMessage = Regex.Replace(strMessage, "[ÀÁÂÃÄÅĄ]", "A");
            strMessage = Regex.Replace(strMessage, "[àáâãäåą]", "a");
            strMessage = Regex.Replace(strMessage, "[ÙÚÛÜ]", "U");
            strMessage = Regex.Replace(strMessage, "[ùúûüµ]", "u");
            strMessage = Regex.Replace(strMessage, "[òóôõöø]", "o");
            strMessage = Regex.Replace(strMessage, "[ÒÓÔÕÖØ]", "O");
            strMessage = Regex.Replace(strMessage, "[ìíîï]", "i");
            strMessage = Regex.Replace(strMessage, "[ÌÍÎÏ]", "I");
            strMessage = Regex.Replace(strMessage, "[ł]", "l");
            strMessage = Regex.Replace(strMessage, "[Ł]", "L");
            strMessage = Regex.Replace(strMessage, "[šś]", "s");
            strMessage = Regex.Replace(strMessage, "[ŠŚ]", "S");
            strMessage = Regex.Replace(strMessage, "[ñń]", "n");
            strMessage = Regex.Replace(strMessage, "[ÑŃ]", "N");
            strMessage = Regex.Replace(strMessage, "[çć]", "c");
            strMessage = Regex.Replace(strMessage, "[ÇĆ]", "C");
            strMessage = Regex.Replace(strMessage, "[ÿ]", "y");
            strMessage = Regex.Replace(strMessage, "[Ÿ]", "Y");
            strMessage = Regex.Replace(strMessage, "[žżź]", "z");
            strMessage = Regex.Replace(strMessage, "[ŽŻŹ]", "Z");
            strMessage = Regex.Replace(strMessage, "[Ð]", "D");
            strMessage = Regex.Replace(strMessage, "[œ]", "oe");
            strMessage = Regex.Replace(strMessage, "[Œ]", "Oe");
            strMessage = Regex.Replace(strMessage, "[«»\u201C\u201D\u201E\u201F\u2033\u2036]", "\"");
            strMessage = Regex.Replace(strMessage, "[\u2026]", "...");
            return strMessage;
        }
        private void encodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(toEncodeInImageFilePathTextBox.Text))
            {
                Bitmap toEncodeInImage = new Bitmap(toEncodeInImageFilePathTextBox.Text);
                int stringToEncodeLengthInBits = stringToEncodeTextBox.Text.Length * 8;
                int useBitsToEncode = ((ComboBoxItem)LSBNumberComboBox.SelectedItem).Content.ToString()[0] - '0';
                if (stringToEncodeLengthInBits > toEncodeInImage.Height * toEncodeInImage.Width * useBitsToEncode)
                {
                    MessageBox.Show("String is to long to encode with such options in choosen picture", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    stringToEncodeTextBox.Text = ReplaceSpecialLetters(stringToEncodeTextBox.Text);
                    BitArray bitsToEncode = new BitArray(Encoding.ASCII.GetBytes(stringToEncodeTextBox.Text));
                    int bitsToEncodeIndex = 0;
                    int imageWidth = toEncodeInImage.Width;
                    int imageHeight = toEncodeInImage.Height;
                    Bitmap encodedImage = new Bitmap(imageWidth, imageHeight);
                    Color pixelToEncode;
                    BitArray[] pixelColorRGBArrays = new BitArray[3];
                    int[] encodedPixelColorRGB = new int[3];
                    for (int widthIndex = 0; widthIndex < imageWidth; widthIndex++)
                    {
                        for (int heightIndex = 0; heightIndex < imageHeight; heightIndex++)
                        {
                            pixelToEncode = toEncodeInImage.GetPixel(widthIndex, heightIndex);
                            if (bitsToEncodeIndex < stringToEncodeLengthInBits)
                            {
                                pixelColorRGBArrays[0] = new BitArray(new byte[] { pixelToEncode.R });
                                pixelColorRGBArrays[1] = new BitArray(new byte[] { pixelToEncode.G });
                                pixelColorRGBArrays[2] = new BitArray(new byte[] { pixelToEncode.B });
                                for (int colorIndex = 0; colorIndex < 3; colorIndex++)
                                {
                                    for (int bitIndex = 0; bitIndex < useBitsToEncode; bitIndex++)
                                    {
                                        if (bitsToEncodeIndex < stringToEncodeLengthInBits)
                                        {
                                            pixelColorRGBArrays[colorIndex][bitIndex] = bitsToEncode[bitsToEncodeIndex];
                                            bitsToEncodeIndex++;
                                        }
                                        else break;
                                    }
                                }
                                for (int bitIndex = 0; bitIndex < 3; bitIndex++)
                                {
                                    pixelColorRGBArrays[bitIndex].CopyTo(encodedPixelColorRGB, bitIndex);
                                }
                                pixelToEncode = Color.FromArgb(encodedPixelColorRGB[0], encodedPixelColorRGB[1], encodedPixelColorRGB[2]);
                            }
                            encodedImage.SetPixel(widthIndex, heightIndex, pixelToEncode);
                        }
                    }
                    encodedImage.Save(saveEncodedImageFilePathTextBox.Text);
                    MessageBox.Show("Text has been successfully encoded");
                    toEncodeInImage.Dispose();
                    encodedImage.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Image choosen to encode does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void toEncodeANDSaveEncodedInImageFilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (toEncodeInImageFilePathTextBox.Text.Length > 0 && saveEncodedImageFilePathTextBox.Text.Length > 0) encodeButton.IsEnabled = true;
            else encodeButton.IsEnabled = false;
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

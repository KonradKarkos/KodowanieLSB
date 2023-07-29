using LSBEncoding.Commands;
using LSBEncoding.Utils;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LSBEncoding.Views.EncoderView
{
    /// <summary>
    /// View model for encoding view
    /// </summary>
    public class EncoderViewModel : BaseViewModel
    {
        /// <inheritdoc/>
        public EncoderViewModel() : base()
        {
            ChooseImageToEncodeFilePathCommand = new BindableCommand(OnChooseToEncodeImageFilePathClick);
        }

        /// <summary>
        /// Variable holding complete filepath of image to have text encoded in
        /// </summary>
        private string toEncodeImageFilePath;
        /// <summary>
        /// Accessor for complete filepath of image to have text encoded in
        /// </summary>
        public string ToEncodeImageFilePath
        {
            get => toEncodeImageFilePath;
            set => SetProperty(ref toEncodeImageFilePath, value);
        }

        /// <summary>
        /// Command fired after clickng on choose image button
        /// </summary>
        public BindableCommand ChooseImageToEncodeFilePathCommand { get; set; }
        /// <summary>
        /// Sets filepaths for image files
        /// </summary>
        private void OnChooseToEncodeImageFilePathClick()
        {
            ToEncodeImageFilePath = SelectImageFilePath();
            EncodedImageFilePath = ToEncodeImageFilePath.Insert(ToEncodeImageFilePath.Length - 4, "Encoded");
        }

        /// <inheritdoc/>
        protected override void OnChooseTextFileClick()
        {
            base.OnChooseTextFileClick();

            if (TxtFilePath.Length > 0)
            {
                using (StreamReader streamReader = new StreamReader(TxtFilePath))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    string loadedLine;
                    while ((loadedLine = streamReader.ReadLine()) != null)
                    {
                        stringBuilder.Append(loadedLine);
                    }
                    MainString = stringBuilder.ToString();
                }
            }
            PrefilledMessageBox.ShowInformation("Successfully loaded text file.");
        }

        /// <inheritdoc/>
        protected override void OnMainActionClick()
        {
            if (!File.Exists(ToEncodeImageFilePath))
            {
                PrefilledMessageBox.ShowError("Image choosen to encode does not exist");
                return;
            }
            using (Bitmap toEncodeInImage = new Bitmap(ToEncodeImageFilePath))
            {
                MainString = ReplaceSpecialLetters(MainString);
                BitArray bitsToEncode = new BitArray(Encoding.ASCII.GetBytes(MainString));
                int useBitsToEncode = SelectedBitNumber;

                if (bitsToEncode.Count > toEncodeInImage.Height * toEncodeInImage.Width * useBitsToEncode)
                {
                    PrefilledMessageBox.ShowError("String is to long to encode with such options in choosen picture");
                    return;
                }

                using (Bitmap encodedImage = EncodeBitsInImage(toEncodeInImage, bitsToEncode, useBitsToEncode))
                {
                    encodedImage.Save(EncodedImageFilePath);
                }

                PrefilledMessageBox.ShowInformation("Text has been successfully encoded");
            }
        }

        /// <summary>
        /// Encodes goven bits in given image
        /// </summary>
        /// <param name="toEncodeInImage">Image for encoding</param>
        /// <param name="bitsToEncode">Message to encode in bits</param>
        /// <param name="useBitsToEncode">Indicator how many bits of every RGB color should be used in encoding</param>
        /// <returns>Provided image with message encoded in it</returns>
        private Bitmap EncodeBitsInImage(Bitmap toEncodeInImage, BitArray bitsToEncode, int useBitsToEncode)
        {
            int imageWidth = toEncodeInImage.Width;
            int imageHeight = toEncodeInImage.Height;
            Bitmap encodedImage = new Bitmap(imageWidth, imageHeight);
            int bitsToEncodeIndex = 0;
            Color pixelToEncode;
            BitArray[] pixelColorRGBArrays = new BitArray[3];
            int[] encodedPixelColorRGB = new int[3];

            for (int widthIndex = 0; widthIndex < imageWidth; widthIndex++)
            {
                for (int heightIndex = 0; heightIndex < imageHeight; heightIndex++)
                {
                    pixelToEncode = toEncodeInImage.GetPixel(widthIndex, heightIndex);
                    if (bitsToEncodeIndex < bitsToEncode.Count)
                    {
                        pixelColorRGBArrays[0] = new BitArray(new byte[] { pixelToEncode.R });
                        pixelColorRGBArrays[1] = new BitArray(new byte[] { pixelToEncode.G });
                        pixelColorRGBArrays[2] = new BitArray(new byte[] { pixelToEncode.B });
                        for (int colorIndex = 0; colorIndex < 3; colorIndex++)
                        {
                            for (int bitIndex = 0; bitIndex < useBitsToEncode; bitIndex++)
                            {
                                if (bitsToEncodeIndex < bitsToEncode.Count)
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

            return encodedImage;
        }

        /// <summary>
        /// Replaces special letters from most known alphabetes to the ASCII ones for esiaer encoding
        /// </summary>
        /// <param name="strMessage">String to encode</param>
        /// <returns>String with replaced special letters</returns>
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
    }
}

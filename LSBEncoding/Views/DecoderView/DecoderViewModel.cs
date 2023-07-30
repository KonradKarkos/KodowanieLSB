using LSBEncoding.Commands;
using LSBEncoding.Utils;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;

namespace LSBEncoding.Views.DecoderView
{
    /// <summary>
    /// View model for decoding view
    /// </summary>
    public class DecoderViewModel : BaseViewModel
    {
        /// <summary>
        /// Number of steps to perform aside of decoding work
        /// </summary>
        private const int ADDITIONAL_WORKER_STEPS = 2;

        /// <inheritdoc/>
        public DecoderViewModel() : base()
        {
            SaveTextToFileCommand = new BindableCommand(OnSaveTextToFileClick);
        }

        /// <inheritdoc/>
        protected override void PerformCoding()
        {
            if (!File.Exists(EncodedImageFilePath))
            {
                PrefilledMessageBox.ShowError("Image with text to decode does not exist");
                return;
            }

            MainString = Encoding.ASCII.GetString(GetDecodedText());
            _worker.ReportProgress(1);

            PrefilledMessageBox.ShowInformation("Decoding finished");
        }

        /// <summary>
        /// Decodes text from encoded image
        /// </summary>
        /// <returns>Decoded text</returns>
        private byte[] GetDecodedText()
        {
            using (Bitmap imageToDecode = new Bitmap(EncodedImageFilePath))
            {
                int readBitsToDecode = SelectedBitNumber;
                int imageWidth = imageToDecode.Width;
                int imageHeight = imageToDecode.Height;
                SetPBValues(imageWidth + ADDITIONAL_WORKER_STEPS);
                ProgressBarVisibility = Visibility.Visible;

                Color pixelToDecode;
                BitArray[] encodedPixelColorRGBArrays = new BitArray[3];
                BitArray decodedBits = new BitArray(imageWidth * imageHeight * readBitsToDecode * 3);
                int bitsToDecodeIndex = 0;
                for (int widthIndex = 0; widthIndex < imageWidth; widthIndex++)
                {
                    for (int heightIndex = 0; heightIndex < imageHeight; heightIndex++)
                    {
                        pixelToDecode = imageToDecode.GetPixel(widthIndex, heightIndex);
                        encodedPixelColorRGBArrays[0] = new BitArray(new byte[] { pixelToDecode.R });
                        encodedPixelColorRGBArrays[1] = new BitArray(new byte[] { pixelToDecode.G });
                        encodedPixelColorRGBArrays[2] = new BitArray(new byte[] { pixelToDecode.B });
                        for (int colorIndex = 0; colorIndex < 3; colorIndex++)
                        {
                            for (int bitIndex = 0; bitIndex < readBitsToDecode; bitIndex++)
                            {
                                decodedBits[bitsToDecodeIndex] = encodedPixelColorRGBArrays[colorIndex][bitIndex];
                                bitsToDecodeIndex++;
                            }
                        }
                    }
                    _worker.ReportProgress(1);
                }
                return GetDecodedText(decodedBits);
            }
        }

        /// <summary>
        /// Gets decoded text from provided decoded bit arrays
        /// </summary>
        /// <param name="decodedBits">Arrays with decoded bits</param>
        /// <returns>Decoded text</returns>
        private byte[] GetDecodedText(BitArray decodedBits)
        {
            byte[] lettersInBites = new byte[decodedBits.Length / 8];
            BitArray letter;
            for (int i = 0; i < lettersInBites.Length; i++)
            {
                letter = new BitArray(new bool[] { decodedBits[i * 8], decodedBits[i * 8 + 1], decodedBits[i * 8 + 2], decodedBits[i * 8 + 3], decodedBits[i * 8 + 4], decodedBits[i * 8 + 5], decodedBits[i * 8 + 6], decodedBits[i * 8 + 7] });
                letter.CopyTo(lettersInBites, i);
            }
            _worker.ReportProgress(1);
            return lettersInBites;
        }

        /// <summary>
        /// Command filed when save decoded text file has been clicked
        /// </summary>
        public BindableCommand SaveTextToFileCommand { get; set; }
        /// <summary>
        /// Saves decoded text to file
        /// </summary>
        private void OnSaveTextToFileClick()
        {
            using (StreamWriter streamWriter = new StreamWriter(TxtFilePath, false, Encoding.UTF8))
            {
                streamWriter.Write(MainString);
            }
            PrefilledMessageBox.ShowInformation("Text saved successfully");
        }
    }
}

using LSBEncoding.Commands;
using LSBEncoding.Utils;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;

namespace LSBEncoding.Views.DecoderView
{
    public class DecoderViewModel : BaseViewModel
    {
        public DecoderViewModel() : base()
        {
            SaveTextToFileCommand = new BindableCommand(OnSaveTextToFileClick);
        }

        protected override void OnMainActionClick()
        {
            if (File.Exists(EncodedImageFilePath))
            {
                using (Bitmap imageToDecode = new Bitmap(EncodedImageFilePath))
                {
                    int readBitsToDecode = SelectedBitNumber;
                    int imageWidth = imageToDecode.Width;
                    int imageHeight = imageToDecode.Height;
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
                    }
                    Byte[] LiteryWBajtach = new Byte[decodedBits.Length / 8];
                    BitArray Litera;
                    for (int i = 0; i < LiteryWBajtach.Length; i++)
                    {
                        Litera = new BitArray(new bool[] { decodedBits[i * 8], decodedBits[i * 8 + 1], decodedBits[i * 8 + 2], decodedBits[i * 8 + 3], decodedBits[i * 8 + 4], decodedBits[i * 8 + 5], decodedBits[i * 8 + 6], decodedBits[i * 8 + 7] });
                        Litera.CopyTo(LiteryWBajtach, i);
                    }
                    MainString = Encoding.ASCII.GetString(LiteryWBajtach);
                }
                PrefilledMessageBox.ShowInformation("Decoding finished");
            }
            else
            {
                PrefilledMessageBox.ShowError("Image with text to decode does not exist");
            }
        }

        public BindableCommand SaveTextToFileCommand { get; set; }

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

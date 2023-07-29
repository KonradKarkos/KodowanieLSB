using LSBEncoding.Commands;
using LSBEncoding.Utils;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace LSBEncoding.Views
{
    /// <summary>
    /// Base view model for encoding and decoding view model
    /// </summary>
    public class BaseViewModel : PropertyNotifierClass
    {
        /// <summary>
        /// Default constructor for view model that initializes basic data
        /// </summary>
        public BaseViewModel()
        {
            HelpCommand = new BindableCommand(OnHelpClick);
            ChooseTextFileCommand = new BindableCommand(OnChooseTextFileClick);
            ChooseEncodedImageFilePathCommand = new BindableCommand(OnChooseEncodedImageFilePathClick);
            MainActionCommand = new BindableCommand(OnMainActionClick);
            SelectedBitNumber = BitComboBoxItems[BitComboBoxItems.Count - 1];
        }

        /// <summary>
        /// Collection of possible bit options to be chosen
        /// </summary>
        public ObservableCollection<int> BitComboBoxItems { get; set; } = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

        /// <summary>
        /// Variable holding number of bits that should be used in encodeing/decoding process
        /// </summary>
        private int selectedBitNumber;
        /// <summary>
        /// Accessor for number of bits that should be used in encodeing/decoding process
        /// </summary>
        public int SelectedBitNumber
        {
            get => selectedBitNumber;
            set => SetProperty(ref selectedBitNumber, value);
        }

        /// <summary>
        /// Variable holding filepath of txt file used to read/write message to encode/decode
        /// </summary>
        private string txtFilePath;
        /// <summary>
        /// Accessor for complete filepath of the txt file
        /// </summary>
        public string TxtFilePath
        {
            get => txtFilePath;
            set => SetProperty(ref txtFilePath, value);
        }

        /// <summary>
        /// Variable with main string for encoding and decoding purposes
        /// </summary>
        private string mainString;
        /// <summary>
        /// Accessor for main input string for encoding and decoding purposes
        /// </summary>
        public string MainString
        {
            get => mainString;
            set => SetProperty(ref mainString, value);
        }

        /// <summary>
        /// Holds complete file path of image with encoded text
        /// </summary>
        private string encodedImageFilePath;
        /// <summary>
        /// Accessor for file path of encoded image
        /// </summary>
        public string EncodedImageFilePath
        {
            get => encodedImageFilePath;
            set => SetProperty(ref encodedImageFilePath, value);
        }

        /// <summary>
        /// Command fired when choose txt file button has been clicked
        /// </summary>
        public BindableCommand ChooseTextFileCommand { get; set; }
        /// <summary>
        /// Action started in result of clicking on choosing text file button
        /// </summary>
        protected virtual void OnChooseTextFileClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                TxtFilePath = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Command fired when user clicks choose encoded image file button
        /// </summary>
        public BindableCommand ChooseEncodedImageFilePathCommand { get; set; }
        /// <summary>
        /// Method assigning choosen filepath to correct variable
        /// </summary>
        private void OnChooseEncodedImageFilePathClick()
        {
            EncodedImageFilePath = SelectImageFilePath();
        }

        /// <summary>
        /// Shows dialog for choosing image file to read
        /// </summary>
        /// <returns>Path of selected image file</returns>
        protected string SelectImageFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp|Tagged Image File Format (*.tif)|*.tif|Joint Photographic Experts Group (*.jpg)|*.jpg|Graphic Interchange Format (*.gif)|*.gif|Portable Network Graphics (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Command fired by main action button of given view
        /// </summary>
        public BindableCommand MainActionCommand { get; set; }
        /// <summary>
        /// Action started in result of clicking on main button in given view
        /// </summary>
        protected virtual void OnMainActionClick() { }

        /// <summary>
        /// Command fired in result of clicking at help button
        /// </summary>
        public BindableCommand HelpCommand { get; set; }
        /// <summary>
        /// Method showing help dialog after firing help command
        /// </summary>
        private void OnHelpClick()
        {
            PrefilledMessageBox.ShowInformation("Application uses Least Sginificant Bit to encode and decode text in/from images converted into bitmap.\n" +
                            "Encoder swaps choosen number of bits of every RGB component of color with text converted to bitarray till all text bits have been swapped - the more bits of every RGB component have been swapped the more will resulting image differ from original one.\n" +
                            "Decoder reads message by reading choosen number of bits from every pixel RGB components and converting them into text.\n" +
                            "All characters outside of ASCII will be changed into most corresponding ASCII characters.");
        }
    }
}

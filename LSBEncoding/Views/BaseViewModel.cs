using System.Collections.ObjectModel;
using System.Windows;
using LSBEncoding.Commands;
using LSBEncoding.Utils;
using Microsoft.Win32;

namespace LSBEncoding.Views
{
    public class BaseViewModel : PropertyNotifierClass
    {
        public BaseViewModel()
        {
            HelpCommand = new BindableCommand(OnHelpClick);
            ChooseTextFileCommand = new BindableCommand(OnChooseTextFileClick);
            ChooseEncodedImageFilePathCommand = new BindableCommand(OnChooseEncodedImageFilePathClick);
            MainActionCommand = new BindableCommand(OnMainActionClick);
            SelectedBitNumber = BitComboBoxItems[BitComboBoxItems.Count - 1];
        }

        public ObservableCollection<int> BitComboBoxItems { get; set; } = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

        private int selectedBitNumber;
        public int SelectedBitNumber
        {
            get => selectedBitNumber;
            set => SetProperty(ref selectedBitNumber, value);
        }

        private string txtFilePath;
        public string TxtFilePath
        {
            get => txtFilePath;
            set => SetProperty(ref txtFilePath, value);
        }

        private string mainString;
        public string MainString
        {
            get => mainString;
            set => SetProperty(ref mainString, value);
        }

        private string encodedImageFilePath;

        public string EncodedImageFilePath
        {
            get => encodedImageFilePath;
            set => SetProperty(ref encodedImageFilePath, value);
        }

        public BindableCommand ChooseTextFileCommand { get; set; }

        protected virtual void OnChooseTextFileClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                TxtFilePath = openFileDialog.FileName;
            }
        }

        public BindableCommand ChooseEncodedImageFilePathCommand { get; set; }

        private void OnChooseEncodedImageFilePathClick()
        {
            EncodedImageFilePath = SelectImageFilePath();
        }

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

        public BindableCommand MainActionCommand { get; set; }

        protected virtual void OnMainActionClick() { }

        public BindableCommand HelpCommand { get; set; }

        private void OnHelpClick()
        {
            PrefilledMessageBox.ShowInformation("Application uses Least Sginificant Bit to encode and decode text in/from images converted into bitmap.\n" +
                            "Encoder swaps choosen number of bits of every RGB component of color with text converted to bitarray till all text bits have been swapped - the more bits of every RGB component have been swapped the more will resulting image differ from original one.\n" +
                            "Decoder reads message by reading choosen number of bits from every pixel RGB components and converting them into text.\n" +
                            "All characters outside of ASCII will be changed into most corresponding ASCII characters.");
        }
    }
}

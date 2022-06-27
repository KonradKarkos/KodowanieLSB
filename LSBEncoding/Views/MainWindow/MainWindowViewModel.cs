using LSBEncoding.Views.DecoderView;
using LSBEncoding.Views.EncoderView;
using System.Windows.Controls;

namespace LSBEncoding.Views.MainWindow
{
    public class MainWindowViewModel : PropertyNotifierClass
    {
        private EncoderViewModel encoderViewModel = new EncoderViewModel();
        private DecoderViewModel decoderViewModel = new DecoderViewModel();

        private BaseViewModel currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }


        private ListViewItem selectedMenuItem;
        public ListViewItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set 
            {
                SetViewModel(value);
                SetProperty(ref selectedMenuItem, value);
            }
        }

        private void SetViewModel(ListViewItem selectedValue)
        {
            switch (selectedValue.Name)
            {
                case "encoderListViewItem":
                    CurrentViewModel = encoderViewModel;
                    break;
                case "decoderListViewItem":
                    CurrentViewModel = decoderViewModel;
                    break;
                default:
                    break;
            }
        }
    }
}

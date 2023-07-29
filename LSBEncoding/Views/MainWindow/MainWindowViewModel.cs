using LSBEncoding.Commands;
using LSBEncoding.Views.DecoderView;
using LSBEncoding.Views.EncoderView;
using System.Windows;
using System.Windows.Controls;

namespace LSBEncoding.Views.MainWindow
{
    /// <summary>
    /// View model for main view holding also menu interaction logic
    /// </summary>
    public class MainWindowViewModel : PropertyNotifierClass
    {
        /// <summary>
        /// View model for encoder view
        /// </summary>
        private EncoderViewModel encoderViewModel = new EncoderViewModel();
        /// <summary>
        /// View model for decoder view
        /// </summary>
        private DecoderViewModel decoderViewModel = new DecoderViewModel();

        /// <summary>
        /// Default constructor that sets basic values
        /// </summary>
        public MainWindowViewModel()
        {
            OpenMenuButtonVisibility = Visibility.Visible;
            CloseMenuButtonVisibility = Visibility.Collapsed;
            ChangeMenuButtonVisibilityCommand = new BindableCommand(OnChangeMenuButtonVisibility);
        }

        /// <summary>
        /// Hold currently displayed viewmodel
        /// </summary>
        private BaseViewModel currentViewModel;
        /// <summary>
        /// Accessor for currently displayed view model
        /// </summary>
        public BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }

        /// <summary>
        /// Holds value of currently selected item on list menu
        /// </summary>
        private ListViewItem selectedMenuItem;
        /// <summary>
        /// Accessor for private property. Also triggers view switch
        /// </summary>
        public ListViewItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set
            {
                SetViewModel(value);
                SetProperty(ref selectedMenuItem, value);
            }
        }
        /// <summary>
        /// Sets currently displayed view by changing bound viewmodel
        /// </summary>
        /// <param name="selectedValue">Value selected in menu list</param>
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

        /// <summary>
        /// Visibility of open menu button
        /// </summary>
        private Visibility _openMenuButtonVisibility;
        /// <summary>
        /// Accessor for private property <see cref="_openMenuButtonVisibility"/>
        /// </summary>
        public Visibility OpenMenuButtonVisibility
        {
            get => _openMenuButtonVisibility;
            set
            {
                SetProperty(ref _openMenuButtonVisibility, value);
            }
        }

        /// <summary>
        /// Visibility of open menu button
        /// </summary>
        private Visibility _closeMenuButtonVisibility;
        /// <summary>
        /// Accessor for private property <see cref="_closeMenuButtonVisibility"/>
        /// </summary>
        public Visibility CloseMenuButtonVisibility
        {
            get => _closeMenuButtonVisibility;
            set
            {
                SetProperty(ref _closeMenuButtonVisibility, value);
            }
        }

        /// <summary>
        /// Command fired by menu buttons
        /// </summary>
        public BindableCommand ChangeMenuButtonVisibilityCommand { get; set; }
        /// <summary>
        /// Switches visibility between close and open menu buttons
        /// </summary>
        public void OnChangeMenuButtonVisibility()
        {
            Visibility tmp = _closeMenuButtonVisibility;
            CloseMenuButtonVisibility = _openMenuButtonVisibility;
            OpenMenuButtonVisibility = tmp;
        }
    }
}

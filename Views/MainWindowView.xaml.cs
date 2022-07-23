using System.Windows;
using System.Windows.Input;
using ToolDemocraci.ViewModel;

namespace ToolDemocraci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public static MainViewModel vm;

        public MainWindowView(MainViewModel _vm)
        {
            InitializeComponent();
            vm = _vm;
            this.DataContext = vm;
            vm.GetRestrict(PremierRes, PartyRes, Answere1, Answere2);
        }

    }
}
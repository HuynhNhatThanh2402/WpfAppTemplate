using WpfAppTemplate.ViewModels;
using System.Windows;

namespace WpfAppTemplate.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // ActualWidth and ActualHeight now reflect the real window size
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            MessageBox.Show(
                $"Window dimensions:\nWidth = {width:N0} px\nHeight = {height:N0} px",
                "Window Size",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
